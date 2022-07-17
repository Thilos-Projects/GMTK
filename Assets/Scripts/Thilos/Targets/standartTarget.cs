using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class standartTarget : MonoBehaviour , ITarget
{
    public Animator animator;

    public targerScriptableOBject prefab;

    public int layer;
    public Vector2Int from;
    public Vector2Int to;

    Vector3 Dir;
    float distance;
    float remainingDist;

    public float leben;

    public float freez = 0;

    public float markedTime = 0;

    public float speedMult = 1;

    public Transform LebensLeiste;

    targetPathGenerator tpg;

    public int getLayer()
    {
        return layer;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }
    
    public Transform getTransform()
    {
        return transform;
    }

    public void applyDamage(ITarget.DamageType type)
    {
        leben -= type.damage * (markedTime > 0 ? 2 : 1);
        StartCoroutine(dotFunct(type.dot * (markedTime > 0 ? 2 : 1), type.dotTime * (markedTime > 0 ? 2 : 1)));
        freez += type.freezTime * (markedTime > 0 ? 2 : 1);
        speedMult *= type.speedMult / (markedTime > 0 ? 2 : 1);
        StartCoroutine(speetMultFunct(type.speedMult / (markedTime > 0 ? 2 : 1), type.speedMultTime * (markedTime > 0 ? 2 : 1)));
        markedTime += type.markedTime * (markedTime > 0 ? 2 : 1);
    }

    IEnumerator dotFunct(float dot, int dotTime)
    {
        for(int i = 0; i < dotTime; i++)
        {
            yield return new WaitForSeconds(1);
            leben -= dot;
        }
    }
    IEnumerator speetMultFunct(float amount, float time)
    {
        yield return new WaitForSeconds(time);
        speedMult /= amount;
    }

    private void Start()
    {
        tpg = targetPathGenerator.getInstance();
        Target();
        TargetManager.addLowerTarget(this);
        leben = prefab.leben;
    }

    void Target()
    {
        Vector3 fromLocal = tpg.tmb.tm.CellToWorld(TileMapBuilder.mapToTile(from)) + new Vector3(0, 0.25f, 0);
        Vector3 toLocal = tpg.tmb.tm.CellToWorld(TileMapBuilder.mapToTile(to)) + new Vector3(0, 0.25f, 0);
        Dir = toLocal - fromLocal;
        Dir.Normalize();
        distance = Vector3.Distance(fromLocal, toLocal);
        remainingDist = distance;
    }

    private void Update()
    {
        LebensLeiste.localScale = new Vector3(LebensLeiste.localScale.z * (leben / prefab.leben), LebensLeiste.localScale.y, LebensLeiste.localScale.z);

        animator.SetFloat("xDir" , Dir.x);
        animator.SetFloat("yDir" , Dir.y);
        if (layer == 1)
        {
            animator.SetBool("On Heads", false);
            animator.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            animator.SetBool("On Heads", true);
            animator.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (freez > 0)
        {
            freez -= Time.deltaTime;
            return;
        }
        else
            freez = 0;

        if (markedTime > 0)
            markedTime -= Time.deltaTime;
        else
            markedTime = 0;

        if(leben < 0)
        {
            TargetManager.remLowerTarget(this);
            TargetManager.remUpperTarget(this);
            Destroy(gameObject);
            return;
        }

        if(remainingDist < prefab.speed * speedMult * Time.deltaTime)
        {
            transform.position = tpg.tmb.tm.CellToWorld(TileMapBuilder.mapToTile(to)) + new Vector3(0, 0.25f, 0);
            if (tpg.hasNext(to, layer))
            {
                from = to;
                to = tpg.getNextTarget(to, layer);
                Target();
            }
            else
                reachedEndOfLine();
        }
        else
        {
            transform.position += Dir * prefab.speed * speedMult * Time.deltaTime;
            remainingDist -= prefab.speed * speedMult * Time.deltaTime;
        }
    }

    private void reachedEndOfLine()
    {
        if (layer == 1)
        {
            from = tpg.upperStart;
            transform.position = tpg.tmb.tm.CellToWorld(TileMapBuilder.mapToTile(from)) + new Vector3(0, 0.25f, 0);
            layer = 0;
            to = tpg.getNextTarget(from, layer);
            Target();
            TargetManager.moveFromLowerToUpper(this);
        }
        else
            SceenManager.getInstance().onEnemyToBase();
    }
}