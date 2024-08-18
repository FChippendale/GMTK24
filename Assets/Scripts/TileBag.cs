using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using System.Runtime.InteropServices;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class TileBag
{
    static bool hasNeighbour(bool[,,] tile, int x, int y, int z)
    {
        return (x > 0 && tile[x - 1, y, z]) ||
                (y > 0 && tile[x, y - 1, z]) ||
                (z > 0 && tile[x, y, z - 1]) ||
                (x < tile.GetLength(0) - 1 && tile[x + 1, y, z]) ||
                (y < tile.GetLength(1) - 1 && tile[x, y + 1, z]) ||
                (z < tile.GetLength(2) - 1 && tile[x, y, z + 1]);
    }

    static void growByOne(List<(int, int, int)> squares, bool[,,] tile)
    {
        List<(int, int, int)> possiblePlacements = new List<(int, int, int)>();

        for (int x = 0; x < tile.GetLength(0); x++)
        {
            for (int y = 0; y < tile.GetLength(1); y++)
            {
                for (int z = 0; z < tile.GetLength(2); z++)
                {
                    if (hasNeighbour(tile, x, y, z) && !tile[x, y, z])
                    {
                        possiblePlacements.Add((x, y, z));
                    }
                }
            }
        }

        var (p_x, p_y, p_z) = possiblePlacements[UnityEngine.Random.Range(0, possiblePlacements.Count)];
        tile[p_x, p_y, p_z] = true;
        squares.Add((p_x, p_y, p_z));
    }

    static public bool[,,] GetRandomShape()
    {
        // y % 2 == 1
        // Up           (1,  0)
        // Up right     (0,  1)
        // Down right   (-1, 1)
        // Down         (-1, 0)
        // Down left    (-1, -1)
        // Up left      (0, -1)

        // y % 2 == 0
        // Up           (1, 0)
        // Up right     (1, 1)
        // Down right   (0, 1)
        // Down         (-1, 0)
        // Down left    (0, -1)
        // Up left      (1, -1)

        // Directions and rotations in cube coordinates.
        //
        // | 0 -1  0 |
        // | 0  0  1 |
        // | 1  0  0 |
        //
        // Direction    Vector          Next rotation   Transform
        // ------------------------------------------------------
        // Up           (0, 1, 0)   ->  (0, 0, 1)       j -> k
        // Up right     (0, 0, 1)   ->  (1, 0, 0)       k -> i
        // Down right   (1, 0, 0)   ->  (0, -1, 0)      i -> -j
        // Down         (0, -1, 0)  ->  (0, 0, -1)      j -> k
        // Down left    (0, 0, -1)  ->  (-1, 0, 0)      k -> i
        // Up left      (-1, 0, 0)  ->  (0, 1, 0)       i -> -j

        List<(int, int, int)> squares = new List<(int, int, int)>();
        bool[,,] tile = new bool[4, 4, 4];
        tile[2, 2, 2] = true;
        squares.Add((2, 2, 2));
        growByOne(squares, tile);
        growByOne(squares, tile);
        growByOne(squares, tile);


        int lowest_x = squares[0].Item1;
        int lowest_y = squares[0].Item2;
        int lowest_z = squares[0].Item3;

        foreach (var (x, y, z) in squares)
        {
            lowest_x = Math.Min(x, lowest_x);
            lowest_y = Math.Min(y, lowest_y);
            lowest_z = Math.Min(z, lowest_z);
        }

        bool[,,] normalized_tile = new bool[4, 4, 4];
        foreach (var (x, y, z) in squares)
        {
            normalized_tile[x - lowest_x, y - lowest_y, z - lowest_z] = true;
        }

        return normalized_tile;
    }

    static public List<(int, int)> ConvertToUnityCoords(bool[,,] tile, int origin_x, int origin_y)
    {
        Dictionary<(int, int, int), (int, int)> even_mapping = new Dictionary<(int, int, int), (int, int)>{
            {(0, 1, 0), (1,  0)},
            {(0, 0, 1), (0,  1)},
            {(1, 0, 0), (-1, 1)},
            {(0, -1, 0), (-1, 0)},
            {(0, 0, -1), (-1, -1)},
            {(-1, 0, 0), (0, -1) },
        };

        Dictionary<(int, int, int), (int, int)> odd_mapping = new Dictionary<(int, int, int), (int, int)>{
            {(0, 1, 0),  (1, 0)},
            {(0, 0, 1),  (1, 1)},
            {(1, 0, 0),  (0, 1)},
            {(0, -1, 0), (-1, 0)},
            {(0, 0, -1), (0, -1)},
            {(-1, 0, 0), (1, -1)},
        };

        List<(int, int)> result = new List<(int, int)>();

        int x_current_x = origin_x;
        int x_current_y = origin_y;

        for (int x = 0; x < tile.GetLength(0); x++)
        {
            int y_current_x = x_current_x;
            int y_current_y = x_current_y;
            for (int y = 0; y < tile.GetLength(1); y++)
            {
                int z_current_x = y_current_x;
                int z_current_y = y_current_y;
                for (int z = 0; z < tile.GetLength(2); z++)
                {
                    if (tile[x, y, z])
                    {
                        result.Add((z_current_x, z_current_y));
                    }
                    if (z_current_y % 2 == 0)
                    {
                        z_current_x += even_mapping[(0, 0, 1)].Item1;
                        z_current_y += even_mapping[(0, 0, 1)].Item2;
                    }
                    else
                    {
                        z_current_x += odd_mapping[(0, 0, 1)].Item1;
                        z_current_y += odd_mapping[(0, 0, 1)].Item2;
                    }
                }

                if (y_current_y % 2 == 0)
                {
                    y_current_x += even_mapping[(0, 1, 0)].Item1;
                    y_current_y += even_mapping[(0, 1, 0)].Item2;
                }
                else
                {
                    y_current_x += odd_mapping[(0, 1, 0)].Item1;
                    y_current_y += odd_mapping[(0, 1, 0)].Item2;
                }
            }

            if (x_current_y % 2 == 0)
            {
                x_current_x += even_mapping[(1, 0, 0)].Item1;
                x_current_y += even_mapping[(1, 0, 0)].Item2;
            }
            else
            {
                x_current_x += odd_mapping[(1, 0, 0)].Item1;
                x_current_y += odd_mapping[(1, 0, 0)].Item2;
            }
        }
        return result;
    }
}
