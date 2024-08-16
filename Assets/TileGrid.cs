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

class Tile
{
    public State state = State.empty;
    public GameObject occupier = null;
}

public class TileGrid
{
    Tile[,] tiles = new Tile[100, 100];


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
        List<(int, int)> offsets = new List<(int, int)>{
            (1, 0),
            (-1, 0),
            (0, 1),
            (0, -1),
            (-1, -1),
            (-1, 1),
        };
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

    public bool TryAddTile(GameObject to_add, int x, int y)
    {
        if (tiles[x, y].state != State.empty)
        {
            return false;
        }

        // Has neighbour?
        if (!hasNeighbourOfState(State.occupied, x, y))
        {
            return false;
        }


        tiles[x, y] = new Tile
        {
            state = State.occupied,
            occupier = to_add
        };

        return true;
    }
}
