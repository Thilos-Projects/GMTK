using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapBuilder : MonoBehaviour
{
    public Tilemap tm;
    public TileBase[] bases;

    void Start()
    {
        tm.ClearAllTiles();
        makeMap();
        printMap();
    }

    //18 hoch
    //17 breit
    public const int width = 17;
    public const int height = 36*2+1;
    public const int startX = 10;
    public const int startY = 25;
    public static int[,] map = new int[width, height];

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
                tm.SetTile(mapToTile(new Vector2Int(x, y)), bases[map[x, y]]);
            }
    }

    public static Vector3Int mapToTile(Vector2Int mapPos)
    {
        return new Vector3Int(startX - mapPos.y + (mapPos.x + mapPos.y / 2), startY - (mapPos.x + mapPos.y / 2), 0);
    }

    public static Vector2Int tileToMap(Vector3Int tilePos)
    {
        //tilePos.x = startX - mapPos.y + (mapPos.x + mapPos.y / 2)
        //tilePos.x - startX + mapPos.y - mapPos.y/2 = mapPos.x

        //tilePos.y = startY - (mapPos.x + mapPos.y/2)
        //tilePos.y - startY + mapPos.x = -mapPos.y/2
        //-(tilePos.y - startY + mapPos.x)*2 = mapPos.y
        //- 2 * tilePos.y + 2 * startY - 2 * mapPos.x = mapPos.y
        //- 2 * tilePos.y + 2 * startY - 2 * (tilePos.x - startX + mapPos.y/2) = mapPos.y
        //- 2 * tilePos.y + 2 * startY -2 * tilePos.x + 2 * startX - mapPos.y = mapPos.y
        //- 2 * tilePos.y + 2 * startY -2 * tilePos.x + 2 * startX = 2 * mapPos.y
        //- tilePos.y + startY - tilePos.x + startX = mapPos.y


        //tilePos.x - startX + mapPos.y/2 = mapPos.x
        //tilePos.x - startX + (- tilePos.y + startY - tilePos.x + startX)/2 = mapPos.x

        int y = -tilePos.y + startY - tilePos.x + startX;

        return new Vector2Int(tilePos.x - startX + y / 2 + y % 2, y);
    }
}