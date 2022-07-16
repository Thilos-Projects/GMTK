using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    public Vector3 getPosition();
    public int getLayer();
}