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
    //wird beim platzieren des turms aufgerufen
    public UnityEvent onSetup;
    //wird auffgerufen, sobald die setup time nach dem setup aufruf vorbei ist
    public UnityEvent onSetupFinisch;

    public bool isShooting;
    //wir mit dem schieﬂ befehl aufgeruen
    public UnityEvent onShoot;
    //wird aufgerufen, sobald der schieﬂcooldown abgelaufen ist
    public UnityEvent onShootFinisch;

    //wir aufgerufen sobald ein neues ziel eingetragen ist
    public UnityEvent onLookOn;
    //wird jeden frame aufgerufen, der das target anvisiert
    public UnityEvent onLookOnIsOn;
    
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
        if(onSetup == null)
            onSetup = new UnityEvent();
        if (onSetupFinisch == null)
            onSetupFinisch = new UnityEvent();
        if (onShoot == null)
            onShoot = new UnityEvent();
        if (onShootFinisch == null)
            onShootFinisch = new UnityEvent();
        if (onLookOn == null)
            onLookOn = new UnityEvent();
        if (onLookOnIsOn == null)
            onLookOnIsOn = new UnityEvent();

        isSetup = false;
        onSetup.Invoke();
        StartCoroutine(setupFinischEnum());
    }
    private void SetupFinisch()
    {
        isSetup = true;
        onSetupFinisch.Invoke();
    }
    IEnumerator setupFinischEnum()
    {
        yield return new WaitForSeconds(prefab.setupTime);
        SetupFinisch();
    }

    public Animator body;

    public void Shoot()
    {
        if (target == null)
            return;
        if(isShooting)
            return;
        isShooting = true;
        BulletAction.Setup(target, transform, prefab.bullet);
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
        if (!isSetup)
            return;
        try
        {
            if (target == null)
            {
                List<ITarget> t = TargetManager.getTargets(transform.position, layer, prefab.range);
                if (t.Count == 0)
                    return;
                setTarget(t[0]);
            }
            else if (Vector2.Distance(target.getPosition(), transform.position) > prefab.range)
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
                body.SetFloat("Direction", viewDirection);
            }
        }catch(MissingReferenceException e)
        {
            target = null;
        }
    }
}