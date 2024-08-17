using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
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
    }

    Dictionary<GameObject, (int, int)> mapping;
    Tile[,] tiles;


    public TileGrid()
    {
        mapping = new Dictionary<GameObject, (int, int)>();
        tiles = new Tile[300, 300];

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y] = new Tile();

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

    List<Tile> getNeighbors(int x, int y)
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

        List<Tile> neighbours = new List<Tile>();
        foreach (var (dir_x, dir_y) in offsets)
        {
            if (x + dir_x >= 0 && x + dir_x < tiles.GetLength(0) && y + dir_y >= 0 && y + dir_y < tiles.GetLength(1))
            {
                neighbours.Add(tiles[x + dir_x, y + dir_y]);
            }
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

    public bool TryAddTile(GameObject to_add, int x, int y, bool allow_island)
    {
        if (tiles[x, y].state != State.empty)
        {
            return false;
        }

        // Has neighbour?
        if (!allow_island && !hasNeighbourOfState(State.occupied, x, y))
        {
            return false;
        }


        tiles[x, y] = new Tile
        {
            state = State.occupied,
            occupier = to_add
        };
        mapping.Add(to_add, (x, y));

        return true;
    }

    public (int, int) GetCenterTile()
    {
        return (tiles.GetLength(0) / 2, tiles.GetLength(1) / 2);
    }
}
