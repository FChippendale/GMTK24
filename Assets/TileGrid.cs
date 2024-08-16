using System.Collections;
using System.Collections.Generic;
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
    Tile[,,] tiles = new Tile[20, 20, 20];


    bool hasNeighbourOfState(State state, int x, int y, int z)
    {
        foreach (Tile neighbour in getNeighbors(x, y, z))
        {
            if (neighbour.state == state)
            {
                return true;
            }
        }
        return false;
    }

    List<Tile> getNeighbors(int x, int y, int z)
    {
        List<Tile> neighbours = new List<Tile>();
        if (x < tiles.GetLength(0))
        {
            neighbours.Add(tiles[x + 1, y, z]);
        }
        if (x > 0)
        {
            neighbours.Add(tiles[x - 1, y, z]);
        }
        if (y < tiles.GetLength(0))
        {
            neighbours.Add(tiles[x, y + 1, z]);
        }
        if (y > 0)
        {
            neighbours.Add(tiles[x, y - 1, z]);
        }
        if (z < tiles.GetLength(0))
        {
            neighbours.Add(tiles[x, y, z + 1]);
        }
        if (z > 0)
        {
            neighbours.Add(tiles[x, y, z - 1]);
        }
        return neighbours;
    }

    List<Tile> getNeighborsOfState(State state, int x, int y, int z)
    {
        List<Tile> neighbours = new List<Tile>();
        foreach (Tile neighbour in getNeighbors(x, y, z))
        {
            if (neighbour.state == state)
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours;
    }

    public List<GameObject> GetNeighbours(int x, int y, int z)
    {
        List<GameObject> neighbours = new List<GameObject>();
        foreach (Tile neighbour in getNeighborsOfState(State.occupied, x, y, z))
        {
            neighbours.Add(neighbour.occupier);
        }
        return neighbours;
    }

    public bool TryAddTile(GameObject to_add, int x, int y, int z)
    {
        if (tiles[x, y, z].state != State.empty)
        {
            return false;
        }

        // Has neighbour?
        if (!hasNeighbourOfState(State.occupied, x, y, y))
        {
            return false;
        }


        tiles[x, y, z] = new Tile
        {
            state = State.occupied,
            occupier = to_add
        };

        return true;
    }


}
