using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="TD/Tower")]
public class towerScriptableObject : ScriptableObject
{
    public int targetCount;
    public bool isBuf;
    public bool isWayTower;
    public float range;
    public float setupTime;
    public float cooldown;
    public float turningSpeed;

    //public Sprite body;
    //public Sprite turret;
    public RuntimeAnimatorController prefab;

    public bulletScriptableObject bullet;
    public bulletScriptableObject bulletUpgraded;

    public towerScriptableObject onPathAlternative;

    public towerScriptableObject reverseTower;
}