using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileMapBuilder))]
public class targetPathGenerator : MonoBehaviour
{
    public static targetPathGenerator instance;
    public static targetPathGenerator getInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    public Transform parrent;

    public TileMapBuilder tmb;

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
        lowerStart = tmb.path.originePos;

        if (openPositions[0] != lowerStart)
            openPositions.Reverse();

        for(int i = 1; i < openPositions.Count; i++)
            if (!directionMapTails.ContainsKey(openPositions[i - 1]))
                directionMapTails.Add(openPositions[i-1], openPositions[i]);

        openPositions = new List<Vector2Int>(tmb.path.positionenOben);
        upperStart = tmb.path.holePos;

        if (openPositions[0] != upperStart)
            openPositions.Reverse();

        for (int i = 1; i < openPositions.Count; i++)
            if (!directionMapHead.ContainsKey(openPositions[i - 1]))
                directionMapHead.Add(openPositions[i - 1], openPositions[i]);
    }

    Dictionary<Vector2Int, Vector2Int> directionMapTails;
    Dictionary<Vector2Int, Vector2Int> directionMapHead;

    public Vector2Int upperStart;
    public Vector2Int lowerStart;

    public bool hasNext(Vector2Int lastTarget, int layer)
    {
        if (layer == 0)
            return directionMapHead.ContainsKey(lastTarget);
        else
            return directionMapTails.ContainsKey(lastTarget);
    }

    public Vector2Int getNextTarget(Vector2Int lastTarget, int layer)
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