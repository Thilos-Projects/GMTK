using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName ="TD/Tower")]
public class towerScriptableObject : ScriptableObject
{
    public bool isWayTower;
    public float range;
    public float setupTime;
    public float cooldown;
    public float turningSpeed;

    //public Sprite body;
    //public Sprite turret;
    public AnimatorController prefab;

    public bulletScriptableObject bullet;
    public towerScriptableObject onPathAlternative;

    public towerScriptableObject reverseTower;
}