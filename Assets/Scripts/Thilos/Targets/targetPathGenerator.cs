using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileMapBuilder))]
public class targetPathGenerator : MonoBehaviour
{
    public Transform parrent;

    TileMapBuilder tmb;

    void Start()
    {
        tmb = GetComponent<TileMapBuilder>();
        directionMapTails = new Dictionary<Vector2Int, Vector2Int>();
        directionMapHead = new Dictionary<Vector2Int, Vector2Int>();
        buildPath();
    }

    void buildPath()
    {
        List<Vector2Int> openPositions = new List<Vector2Int>(tmb.path.positionenUnten);
        Vector2Int currentPos = tmb.path.originePos;
        lowerStart = currentPos;
        Vector2Int nextPosition = Vector2Int.zero;
        while (openPositions.Count > 0)
        {
            float dist = float.PositiveInfinity;
            for (int i = 0; i < openPositions.Count; i++)
                if (dist > Vector2Int.Distance(currentPos, openPositions[i]))
                {
                    dist = Vector2Int.Distance(currentPos, openPositions[i]);
                    nextPosition = openPositions[i];
                }
            if (!directionMapTails.ContainsKey(currentPos))
                directionMapTails.Add(currentPos, nextPosition);
            currentPos = nextPosition;
            openPositions.Remove(currentPos);
            Debug.Log(dist);
        }

        openPositions = new List<Vector2Int>(tmb.path.positionenOben);
        currentPos = tmb.path.holePos;
        upperStart = currentPos;
        nextPosition = Vector2Int.zero;
        while (openPositions.Count > 0)
        {
            float dist = float.PositiveInfinity;
            for (int i = 0; i < openPositions.Count; i++)
                if (dist > Vector2Int.Distance(currentPos, openPositions[i]))
                {
                    dist = Vector2Int.Distance(currentPos, openPositions[i]);
                    nextPosition = openPositions[i];
                }
            if (!directionMapHead.ContainsKey(currentPos))
                directionMapHead.Add(currentPos, nextPosition);
            currentPos = nextPosition;
            openPositions.Remove(currentPos);
            Debug.Log(dist);
        }
    }

    Dictionary<Vector2Int, Vector2Int> directionMapTails;
    Dictionary<Vector2Int, Vector2Int> directionMapHead;

    public Vector2Int upperStart;
    public Vector2Int lowerStart;

    bool hasNext(Vector2Int lastTarget, int layer)
    {
        if (layer == 0)
            return directionMapHead.ContainsKey(lastTarget);
        else
            return directionMapTails.ContainsKey(lastTarget);
    }

    Vector2Int getNextTarget(Vector2Int lastTarget, int layer)
    {
        if (layer == 0)
        {
            if (directionMapHead.ContainsKey(lastTarget))
                return directionMapHead[lastTarget];
        }
        else
        {
            if (directionMapTails.ContainsKey(lastTarget))
                return directionMapTails[lastTarget];
        }
            
        return Vector2Int.zero;
    }
}