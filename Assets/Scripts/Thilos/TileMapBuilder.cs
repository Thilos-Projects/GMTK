using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileMapBuilder : MonoBehaviour
{
    Tilemap tm;
    public TileBase[] bases;
    void Start()
    {
        tm = GetComponent<Tilemap>();
        tm.ClearAllTiles();
        makeMap();
        printMap();
    }
    //18 hoch
    //17 breit
    const int width = 17;
    const int height = 36*2+1;
    const int startX = 10;
    const int startY = 25;
    public int[,] map = new int[width, height];

    public void makeMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height / 2; y++)
                map[x, y] = y % 2;
            for (int y = height / 2; y < height; y++)
                map[x, y] = y % 2 + 2;
        }
    }

    public void printMap()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                tm.SetTile(new Vector3Int(startX - y + (x + y/2), startY - (x + y/2), 0), bases[map[x, y]]);
            }
    }
}