using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class towerAction : MonoBehaviour
{
    public towerScriptableObject prefab;

    public int layer;
    //public Vector2 pos;
    public float viewDirection;

    public bool isSetup = false;

    public bool isShooting;
    
    //read only
    public ITarget target;
    public void setTarget(ITarget target)
    {
        this.target = target;
    }

    //Setup call
    public void Setup()
    {
        isSetup = false;
        StartCoroutine(setupFinischEnum());
    }
    private void SetupFinisch()
    {
        isSetup = true;
    }
    IEnumerator setupFinischEnum()
    {
        yield return new WaitForSeconds(prefab.setupTime);
        SetupFinisch();
    }

    public Animator body;
    public bool Upgraded;
    public bool isBuffed;

    public UnityEvent onShoot;

    public void Shoot()
    {
        if (target == null)
            return;
        if(isShooting)
            return;
        isShooting = true;

        if (onShoot != null)
            onShoot.Invoke();

        if(Upgraded)
            BulletAction.Setup(target, transform, prefab.bulletUpgraded);
        else
            BulletAction.Setup(target, transform, prefab.bullet);
        StartCoroutine(ShotFinischEnum());
    }
    private void shotFinisched()
    {
        isShooting = false;
    }
    IEnumerator ShotFinischEnum()
    {
        yield return new WaitForSeconds(prefab.cooldown);
        shotFinisched();
    }

    private void Update()
    {
        if (!isSetup)
            return;
        try
        {
            if (prefab.isBuf)
            {
                for (int x = 0; x < TileMapBuilder.width; x++)
                    for (int y = 0; y < TileMapBuilder.height; y++)
                    {
                        towerAction ta = TowerPlacer.towerMap[x, y];
                        if (ta != null && ta != this)
                            if (Vector3.Distance(ta.transform.position, transform.position) < prefab.range * (Upgraded ? 2 : 1))
                                ta.isBuffed = true;
                    }
                return;
            }

            if(prefab.targetCount > 0)
            {
                if (isShooting)
                    return;

                List<ITarget> t = TargetManager.getTargets(transform.position, layer, prefab.range * (isBuffed ? 2 : 1));
                if (t.Count == 0)
                    return;

                isShooting = true;
                if (Upgraded)
                    for(int i = 0; i < t.Count && i < prefab.targetCount; i++)
                        BulletAction.Setup(t[i], transform, prefab.bulletUpgraded);
                else
                    for (int i = 0; i < t.Count && i < prefab.targetCount; i++)
                        BulletAction.Setup(t[i], transform, prefab.bullet);

                StartCoroutine(ShotFinischEnum());
                return;
            }

            if (target == null)
            {
                List<ITarget> t = TargetManager.getTargets(transform.position, layer, prefab.range * (isBuffed ? 2 : 1));
                if (t.Count == 0)
                    return;
                setTarget(t[0]);
            }
            else if (Vector2.Distance(target.getPosition(), transform.position) > prefab.range * (isBuffed ? 2 : 1))
            {
                target = null;
            }
            else
            {
                float requiredAngle = Vector2.SignedAngle(Vector2.up, target.getPosition() - transform.position) % 360;
                //float requiredAngle = Vector2.Angle(Vector2.up, target.getPosition() - pos);
                float correction = requiredAngle - viewDirection;
                if (correction > 180 || correction < 0)
                {
                    correction = 360 - correction;
                    if (correction > prefab.turningSpeed * Time.deltaTime)
                        viewDirection = (viewDirection - prefab.turningSpeed * Time.deltaTime) % 360;
                    else
                    {
                        viewDirection = requiredAngle;
                        Shoot();
                    }
                }
                else
                {
                    if (correction > prefab.turningSpeed * Time.deltaTime)
                        viewDirection = (viewDirection + prefab.turningSpeed * Time.deltaTime) % 360;
                    else
                    {
                        viewDirection = requiredAngle;
                        Shoot();
                    }
                }
                body.SetFloat("Direction", viewDirection);
            }
        }catch(MissingReferenceException e)
        {
            target = null;
        }
    }
}