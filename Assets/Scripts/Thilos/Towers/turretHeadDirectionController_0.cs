using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretHeadDirectionController_0 : MonoBehaviour
{
    public towerAction towerActionScript;
    private void Update()
    {
        transform.localRotation = Quaternion.AngleAxis(towerActionScript.viewDirection, Vector3.forward);
    }
}