using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAction : MonoBehaviour
{
    public Transform origine;
    public Transform target;

    public bulletScriptableObject prefab;
    
    SpriteRenderer targetRenderer;
    SpriteRenderer origineRenderer;
    LineRenderer targetEffect;

    public float time;

    private void Start()
    {
        target.parent.GetComponent<ITarget>().applyDamage(prefab.damage);
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > prefab.AnimationTimeTarget && targetRenderer != null)
        {
            DestroyImmediate(targetRenderer);
            targetRenderer = null;
        }
        if (time > prefab.AnimationTimeTurret && origineRenderer != null)
        {
            DestroyImmediate(origineRenderer);
            origineRenderer = null;
        }
        if (time > prefab.AnimationTimeLine && targetEffect != null)
        {
            DestroyImmediate(targetEffect);
            targetEffect = null;
        }

        if (targetRenderer != null)
            targetRenderer.sprite = prefab.targetImage[(int)((prefab.AnimationTimeTarget / prefab.targetImage.Length) * time)];
        if (origineRenderer != null)
            origineRenderer.sprite = prefab.turretImage[(int)((prefab.AnimationTimeTurret / prefab.turretImage.Length) * time)];
        if (targetEffect != null)
            targetEffect.colorGradient = prefab.lineColor[((int)((prefab.AnimationTimeLine / prefab.lineColor.Length) * time)) % prefab.lineColor.Length];

        if (targetRenderer == null && origineRenderer == null && targetEffect == null)
        {
            Destroy(this);
            Destroy(origine.gameObject);
            Destroy(target.gameObject);
        }
    }

    public static void Setup(ITarget target, Transform origine, bulletScriptableObject prefab, int recursiveCount = int.MaxValue)
    {
        BulletAction temp = origine.gameObject.AddComponent<BulletAction>();
        temp.prefab = prefab;

        temp.origine = (new GameObject()).transform;
        temp.origine.parent = origine;
        temp.origine.localPosition = Vector3.zero;
        temp.origineRenderer = temp.origine.gameObject.AddComponent<SpriteRenderer>();

        temp.target = (new GameObject()).transform;
        temp.target.parent = target.getTransform();
        temp.target.localPosition = Vector3.zero;
        temp.targetRenderer = temp.target.gameObject.AddComponent<SpriteRenderer>();

        temp.targetEffect = temp.origine.gameObject.AddComponent<LineRenderer>();
        temp.targetEffect.colorGradient = prefab.lineColor[0];
        temp.targetEffect.positionCount = 2;
        temp.targetEffect.SetPosition(0, temp.origine.position);
        temp.targetEffect.SetPosition(1, temp.target.position);
        temp.targetEffect.startWidth = prefab.lineWidth;
        temp.targetEffect.endWidth = prefab.lineWidth;
        temp.targetEffect.material = prefab.lineMat;

        temp.time = 0;

        origine.GetComponent<MonoBehaviour>().StartCoroutine(aoeTrigger(origine.GetComponent<MonoBehaviour>(), target, prefab, recursiveCount));
    }

    public static IEnumerator aoeTrigger(MonoBehaviour callOn, ITarget target, bulletScriptableObject prefab, int recursionCount)
    {
        yield return new WaitForSeconds(prefab.aoeDelay);
        if (prefab.aoeChainWidth != 0 && prefab.aoeChainLength != 0 && recursionCount > 0)
        {
            Vector3 pos = target.getPosition();
            pos.z = 0;
            List<ITarget> temp = TargetManager.getTargets(pos, target.getLayer(), prefab.aoeRange);
            temp.Remove(target);
            for (int i = 0; i < temp.Count && i < prefab.aoeChainWidth; i++)
                Setup(temp[i], target.getTransform(), prefab, Mathf.Min(prefab.aoeChainLength - 1, recursionCount - 1));
        }
    }
}