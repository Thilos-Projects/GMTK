#if UNITY_EDITOR_64

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(TileMapBuilder))]
public class PathEditor : MonoBehaviour
{
    TileMapBuilder tm;

    public enum setAble
    {
        none,
        path,
        hole,
        target
    }

    public setAble toSet;

    void Start()
    {
        tm = GetComponent<TileMapBuilder>();
        InputManager.get().requestInput("ClearPath", clearPath, true, false, false);
        InputManager.get().requestInput("PlaceElement", OnClickPath, false, false, true);
        InputManager.get().requestInput("PlaceElement", onClick, true, false, false);
    }

    private void OnDestroy()
    {
        EditorUtility.SetDirty(tm.path);
    }

    void onClick()
    {
        Vector2Int mapPos = TileMapBuilder.tileToMap(tm.tm.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));

        bool isBottom = mapPos.y > TileMapBuilder.height / 2;

        switch (toSet)
        {
            case setAble.hole:
                {
                    if (!isBottom)
                        tm.path.holePos = mapPos;
                }
                break;
            case setAble.target:
                {
                    if (isBottom)
                        tm.path.originePos = mapPos;
                    else
                        tm.path.targetPos = mapPos;
                }
                break;
            case setAble.none:
                {
                    if (isBottom)
                    {
                        if (tm.path.positionenUnten.Contains(mapPos))
                            tm.path.positionenUnten.Remove(mapPos);
                    }
                    else
                    {
                        if (tm.path.positionenOben.Contains(mapPos))
                            tm.path.positionenOben.Remove(mapPos);
                    }
                }
                break;
        }

        Debug.Log(mapPos);

        tm.makeMap();
        tm.printMap();
    }

    void clearPath()
    { 
        tm.path.positionenOben.Clear();
        tm.path.positionenUnten.Clear();
        tm.makeMap();
        tm.printMap();
    }

    void OnClickPath()
    {
        if (toSet == setAble.path)
        {
            Vector2Int mapPos = TileMapBuilder.tileToMap(tm.tm.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));

            bool isBottom = mapPos.y > TileMapBuilder.height / 2;

            if (isBottom)
            {
                if (tm.path.positionenUnten.Contains(mapPos))
                    return;
                tm.path.positionenUnten.Add(mapPos);
            }
            else
            {
                if (tm.path.positionenOben.Contains(mapPos))
                    return;
                tm.path.positionenOben.Add(mapPos);
            }

            tm.makeMap();
            tm.printMap();
        }
    }
}
#endif