using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapBuilder : MonoBehaviour
{
    public enum TileTypes : byte
    {
        ground = 0,
        HeadPath = 4,
        HeadHole = 5,
        HeadTarget = 6,
        TailPath = 7,
        TailHole = 8,
        TailOrigine = 9
    }

    public Tilemap tm;
    public TileBase[] bases;

    public PathScriptableObject path;

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
                if (path.positionenOben.Contains(new Vector2Int(x, y)))
                    map[x, y] = ((int)TileTypes.HeadPath);
                else
                    map[x, y] = ((int)TileTypes.ground);
            for (int y = height / 2; y < height; y++)
                if (path.positionenUnten.Contains(new Vector2Int(x, y)))
                    map[x, y] = ((int)TileTypes.TailPath);
                else
                    map[x, y] = ((int)TileTypes.ground);
        }
        map[path.targetPos.x, path.targetPos.y] = ((int)TileTypes.HeadTarget);
        map[path.originePos.x, path.originePos.y] = ((int)TileTypes.TailOrigine);
        map[path.holePos.x, path.holePos.y] = ((int)TileTypes.HeadHole);
        map[path.holePos.x, 30 - path.holePos.y + 38] = ((int)TileTypes.TailHole);
    }

    public void printMap()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Vector3Int pos = mapToTile(new Vector2Int(x, y));
                if (y < height / 2)
                    tm.SetTile(pos, bases[(y % 2)]);
                else
                    tm.SetTile(pos, bases[(y % 2 + 2)]);

                if (map[x, y] == ((int)TileTypes.ground))
                    continue;

                if (map[x, y] == ((int)TileTypes.HeadHole))
                {
                    pos.z = 2;
                    tm.SetTile(pos, bases[5]);
                    pos.z = 1;
                    tm.SetTile(pos, bases[4]);
                }
                if (map[x, y] == ((int)TileTypes.TailHole))
                {
                    pos.z = 2;
                    tm.SetTile(pos, bases[8]);
                    pos.z = 1;
                    tm.SetTile(pos, bases[7]);
                }
                if (map[x, y] == ((int)TileTypes.HeadPath))
                {
                    pos.z = 1;
                    tm.SetTile(pos, bases[4]);
                }
                if (map[x, y] == ((int)TileTypes.TailPath))
                {
                    pos.z = 1;
                    tm.SetTile(pos, bases[7]);
                }
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