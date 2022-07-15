using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetDebugBehaiviout : MonoBehaviour, ITarget
{
    private void Start()
    {
        InputManager.get().requestInput("DebugSetPosition", setPosition, false, false, true);
        TargetManager.addUpperTarget(this);
    }

    void setPosition()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        temp.z = 0;
        transform.position = temp;
    }

    public Vector2 getPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
}