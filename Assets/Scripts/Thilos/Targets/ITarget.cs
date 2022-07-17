using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    [System.Serializable]
    public struct DamageType
    {
        public float damage;
        public float dot;
        public int dotTime;
        public float speedMult;
        public float speedMultTime;
        public float markedTime;
        public float freezTime;
    };

    public void applyDamage(DamageType type);
    public Vector3 getPosition();
    public int getLayer();

    public Transform getTransform();
}