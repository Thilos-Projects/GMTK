using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(towerAction))]
public class towerLoadScript : MonoBehaviour
{
    public towerScriptableObject prefab;

    public SpriteRenderer bodySprite;
    public SpriteRenderer turretSprite;

    towerAction actionScript;

    void Start()
    {
        actionScript = gameObject.GetComponent<towerAction>();
        actionScript.layer = 0;
        actionScript.pos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        actionScript.viewDirection = 0;
        actionScript.prefab = prefab;
        bodySprite.sprite = prefab.body;
        turretSprite.sprite = prefab.turret;
        actionScript.Setup();
    }
}