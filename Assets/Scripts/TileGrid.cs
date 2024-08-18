using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

enum State
{
    empty,
    unusable,
    occupied,
}

public class TileGrid
{
    class Tile
    {
        public State state = State.empty;
        public GameObject occupier = null;
        public FactoryBehaviour.TraversalType type = FactoryBehaviour.TraversalType.constant_integer_amount;
    }

    Dictionary<GameObject, (int, int)> mapping;
    Tile[,] tiles;
    HashSet<(int, int)> allTiles;


    public TileGrid()
    {
        mapping = new Dictionary<GameObject, (int, int)>();
        tiles = new Tile[300, 300];
        allTiles = new HashSet<(int, int)>();

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y] = new Tile();
                allTiles.Add((x, y));

                if (x == 0 || x == tiles.GetLength(0) - 1)
                {
                    tiles[x, y].state = State.unusable;
                }
                if (y == 0 || y == tiles.GetLength(1) - 1)
                {
                    tiles[x, y].state = State.unusable;
                }
            }
        }
    }

    bool hasNeighbourOfState(State state, int x, int y)
    {
        foreach (Tile neighbour in getNeighbors(x, y))
        {
            if (neighbour.state == state)
            {
                return true;
            }
        }
        return false;
    }

    List<(int, int)> getNeighborLocs(int x, int y)
    {
        List<(int, int)> offsets;
        if (y % 2 == 0)
        {
            offsets = new List<(int, int)>{
                (-1, 0),
                (1, 0),
                (1, 1),
                (0, 1),
                (0, -1),
                (1, -1),
            };
        }
        else
        {
            offsets = new List<(int, int)>{
                (-1, 0),
                (-1, -1),
                (0, -1),
                (1, 0),
                (0, 1),
                (-1, 1),
            };
        }

        List<(int, int)> neighbours = new List<(int, int)>();
        foreach (var (dir_x, dir_y) in offsets)
        {
            if (x + dir_x >= 0 && x + dir_x < tiles.GetLength(0) && y + dir_y >= 0 && y + dir_y < tiles.GetLength(1))
            {
                neighbours.Add((x + dir_x, y + dir_y));
            }
        }
        return neighbours;
    }

    List<Tile> getNeighbors(int x, int y)
    {
        List<Tile> neighbours = new List<Tile>();
        foreach (var (n_x, n_y) in getNeighborLocs(x, y))
        {
            neighbours.Add(tiles[n_x, n_y]);
        }
        return neighbours;
    }

    List<Tile> getNeighborsOfState(State state, int x, int y)
    {
        List<Tile> neighbours = new List<Tile>();
        foreach (Tile neighbour in getNeighbors(x, y))
        {
            if (neighbour.state == state)
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours;
    }

    List<Tile> getNeighborsNotOfType(FactoryBehaviour.TraversalType type, int x, int y)
    {
        List<Tile> neighbours = new List<Tile>();
        foreach (Tile neighbour in getNeighbors(x, y))
        {
            if (neighbour.type == type)
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours;
    }

    public List<GameObject> GetNeighbours(int x, int y)
    {
        List<GameObject> neighbours = new List<GameObject>();
        foreach (Tile neighbour in getNeighborsOfState(State.occupied, x, y))
        {
            neighbours.Add(neighbour.occupier);
        }
        return neighbours;
    }

    public (int, int) GetLocation(GameObject gameObject)
    {
        return mapping[gameObject];
    }

    public bool TryAddTile(List<GameObject> to_add, List<(int, int)> positions, bool allow_island)
    {
        bool has_neighbour = false;
        foreach (var (x, y) in positions)
        {
            if (tiles[x, y].state != State.empty)
            {
                return false;
            }

            if (hasNeighbourOfState(State.occupied, x, y))
            {
                has_neighbour = true;
            }
        }

        if (!allow_island && !has_neighbour)
        {
            return false;
        }

        int index = 0;
        foreach (var (x, y) in positions)
        {
            tiles[x, y] = new Tile
            {
                state = State.occupied,
                occupier = to_add[index],
                type = to_add[index].GetComponent<FactoryBehaviour>().traversalType,
            };
            mapping.Add(to_add[index], (x, y));
            index += 1;
        }

        return true;
    }

    public List<(int, int)> GetPossiblePlacements()
    {
        List<(int, int)> result = new List<(int, int)>();
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[x, y].state != State.empty)
                {
                    continue;
                }
                if (!hasNeighbourOfState(State.occupied, x, y))
                {
                    continue;
                }
                result.Add((x, y));
            }
        }
        return result;
    }

    public HashSet<(int, int)> GetTilesEncircledBy(FactoryBehaviour.TraversalType type)
    {
        HashSet<(int, int)> CellsExplored = new HashSet<(int, int)>();
        Queue<(int, int)> CellsToExplore = new Queue<(int, int)>();

        // Assume boundaries are never encircled
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            CellsToExplore.Enqueue((x, 0));
            CellsToExplore.Enqueue((x, tiles.GetLength(1) - 1));
        }
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            CellsToExplore.Enqueue((0, y));
            CellsToExplore.Enqueue((tiles.GetLength(0) - 1, y));
        }

        CellsExplored.AddRange(CellsToExplore);

        while (CellsToExplore.Count > 0)
        {
            var (x, y) = CellsToExplore.Dequeue();
            if (tiles[x, y].state == State.occupied && tiles[x, y].type == type)
            {
                continue;
            }

            foreach ((int, int) loc in getNeighborLocs(x, y))
            {
                if (!CellsExplored.Contains(loc))
                {
                    CellsToExplore.Enqueue(loc);
                    CellsExplored.Add(loc);
                }
            }
        }

        return Enumerable.ToHashSet(allTiles.Except(CellsExplored));
    }

    public HashSet<(int, int)> ExpandRingIntoTouching(HashSet<(int, int)> tile_set, FactoryBehaviour.TraversalType type)
    {
        HashSet<(int, int)> cellsExplored = new HashSet<(int, int)>(tile_set);
        Queue<(int, int)> cellsToExplore = new Queue<(int, int)>(tile_set);

        while (cellsToExplore.Count > 0)
        {
            var (x, y) = cellsToExplore.Dequeue();
            List<(int, int)> neighbours = getNeighborLocs(x, y);
            foreach (var (n_x, n_y) in neighbours)
            {
                if (tiles[n_x, n_y].state == State.occupied && tiles[n_x, n_y].type == type)
                {
                    if (!cellsExplored.Contains((n_x, n_y)))
                    {
                        cellsToExplore.Enqueue((n_x, n_y));
                        cellsExplored.Add((n_x, n_y));
                    }
                }
            }
        }
        return cellsExplored;
    }

    public HashSet<(int, int)> ExpandByRing(HashSet<(int, int)> tile_set)
    {
        HashSet<(int, int)> result = new HashSet<(int, int)>(tile_set);
        foreach (var (x, y) in tile_set)
        {
            List<(int, int)> neighbours = getNeighborLocs(x, y);
            foreach ((int, int) loc in neighbours)
            {
                if (!result.Contains(loc))
                {
                    result.Add(loc);
                }
            }
        }
        return result;
    }

    public GameObject GetOccupier(int x, int y)
    {
        return tiles[x, y].occupier;
    }

    public bool HasOccupier(int x, int y)
    {
        return tiles[x, y].state == State.occupied;
    }

    public void RemoveTile(int x, int y)
    {
        if (tiles[x, y].state == State.empty)
        {
            return;
        }

        GameObject occupier = tiles[x, y].occupier;
        tiles[x, y].state = State.empty;
        tiles[x, y].occupier = null;

        mapping.Remove(occupier);
    }

    public (int, int) GetCenterTile()
    {
        return (tiles.GetLength(0) / 2, tiles.GetLength(1) / 2);
    }
}
