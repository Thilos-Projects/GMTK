using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class standartTarget : MonoBehaviour , ITarget
{
    public targerScriptableOBject prefab;

    public int layer;
    public Vector2Int from;
    public Vector2Int to;

    Vector3 Dir;
    float distance;
    float remainingDist;

    targetPathGenerator tpg;

    public int getLayer()
    {
        return layer;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    private void Start()
    {
        tpg = targetPathGenerator.getInstance();
        Target();
        TargetManager.addLowerTarget(this);
    }

    void Target()
    {
        Vector3 fromLocal = tpg.tmb.tm.CellToWorld(TileMapBuilder.mapToTile(from)) + new Vector3(0, 0.25f, 0);
        Vector3 toLocal = tpg.tmb.tm.CellToWorld(TileMapBuilder.mapToTile(to)) + new Vector3(0, 0.25f, 0);
        Dir = toLocal - fromLocal;
        Dir.Normalize();
        distance = Vector3.Distance(fromLocal, toLocal);
        remainingDist = distance;
    }

    private void Update()
    {
        if(remainingDist < prefab.speed * Time.deltaTime)
        {
            transform.position = tpg.tmb.tm.CellToWorld(TileMapBuilder.mapToTile(to)) + new Vector3(0, 0.25f, 0);
            if (tpg.hasNext(to, layer))
            {
                from = to;
                to = tpg.getNextTarget(to, layer);
                Target();
            }
            else
                reachedEndOfLine();
        }
        else
        {
            transform.position += Dir * prefab.speed * Time.deltaTime;
            remainingDist -= prefab.speed * Time.deltaTime;
        }
    }

    private void reachedEndOfLine()
    {
        if(layer == 1)
        {
            from = tpg.upperStart;
            transform.position = tpg.tmb.tm.CellToWorld(TileMapBuilder.mapToTile(from)) + new Vector3(0, 0.25f, 0);
            layer = 0;
            to = tpg.getNextTarget(from, layer);
            Target();
            TargetManager.moveFromLowerToUpper(this);
        }
        else
        {
        }
    }
}