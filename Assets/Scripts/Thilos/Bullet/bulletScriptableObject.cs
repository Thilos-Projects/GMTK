using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="TD/Bullet")]
public class bulletScriptableObject : ScriptableObject
{
    public int aoeChainLength;
    public int aoeChainWidth;
    public float aoeDelay;
    public float aoeRange;
    public float AnimationTimeTurret;
    public Sprite[] turretImage;
    public float AnimationTimeTarget;
    public Sprite[] targetImage;
    public float AnimationTimeLine;
    public Gradient[] lineColor;
    public Material lineMat;
    public float lineWidth;

    public ITarget.DamageType damage;
}