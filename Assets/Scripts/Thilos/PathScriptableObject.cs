using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="TD/Path")]
public class PathScriptableObject : ScriptableObject
{
    public Vector2Int holePos;
    public Vector2Int originePos;
    public Vector2Int targetPos;
    public List<Vector2Int> positionenOben;
    public List<Vector2Int> positionenUnten;
}