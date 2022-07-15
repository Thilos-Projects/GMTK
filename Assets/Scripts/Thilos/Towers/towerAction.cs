using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class towerAction : MonoBehaviour
{
    towerScriptableObject prefab;

    public int layer;
    public Vector2Int pos;
    public float viewDirection;

    public bool isSettingup;
    //wird beim platzieren des turms aufgerufen
    UnityEvent onSetup;
    //wird auffgerufen, sobald die setup time nach dem setup aufruf vorbei ist
    UnityEvent onSetupFinisch;

    public bool isShooting;
    //wir mit dem schieﬂ befehl aufgeruen
    UnityEvent onShoot;
    //wird aufgerufen, sobald der schieﬂcooldown abgelaufen ist
    UnityEvent onShootFinisch;

    //wir aufgerufen sobald ein neues ziel eingetragen ist
    UnityEvent onLookOn;
    //wird jeden frame aufgerufen, der das target anvisiert
    UnityEvent onLookOnIsOn;
    
    //read only
    public ITarget target;
    public void setTarget(ITarget target)
    {
        this.target = target;
        onLookOn.Invoke();
    }

    //Setup call
    public void Setup()
    {
        isSettingup = true;
        onSetup.Invoke();
        StartCoroutine(setupFinischEnum());
    }
    private void SetupFinisch()
    {
        isSettingup = false;
        onSetupFinisch.Invoke();
    }
    IEnumerator setupFinischEnum()
    {
        yield return new WaitForSeconds(prefab.setupTime);
        SetupFinisch();
    }

    public void Shoot()
    {
        if(isShooting)
            return;
        isShooting = true;
        onShoot.Invoke();
        StartCoroutine(ShotFinischEnum());
    }
    private void shotFinisched()
    {
        isShooting = false;
        onShootFinisch.Invoke();
    }
    IEnumerator ShotFinischEnum()
    {
        yield return new WaitForSeconds(prefab.cooldown);
        shotFinisched();
    }

    private void Update()
    {
        if (isSettingup)
            return;
        if (target == null)
        {
            List<ITarget> t = TargetManager.getTargets(pos, layer, prefab.range);
            if(t.Count == 0)
                return;
            setTarget(t[0]);
        }
        else
        {
            float requiredAngle = Vector2.Angle(Vector2.up, target.getPosition() - pos) % 360;
            float correction = requiredAngle - viewDirection;
            if(correction > 180 || correction < 0)
            {
                correction = 360 - correction;
                if (correction > prefab.turningSpeed * Time.deltaTime)
                    viewDirection = (viewDirection - prefab.turningSpeed * Time.deltaTime) % 360;
                else
                {
                    viewDirection = requiredAngle;
                    onLookOnIsOn.Invoke();
                }
            }
            else
            {
                if (correction > prefab.turningSpeed * Time.deltaTime)
                    viewDirection = (viewDirection + prefab.turningSpeed * Time.deltaTime) % 360;
                else
                {
                    viewDirection = requiredAngle;
                    onLookOnIsOn.Invoke();
                }
            }
        }
    }
}