using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{

    [System.Serializable]
    public struct SpawnEntry
    {
        public float delay;
        public targerScriptableOBject[] toSpawn;
    }

    public List<SpawnEntry> toSpawn;

    public GameObject targetPrefab;
    public Transform parrent;

    private void Start()
    {
        if (toSpawn.Count > 0)
            StartCoroutine(spawnDelay());
        else
            Done();
    }

    void Spawn(targerScriptableOBject toSpawn)
    {
        Transform temp = Instantiate(targetPrefab).transform;
        temp.parent = parrent;
    }

    IEnumerator spawnDelay()
    {
        SpawnEntry entry = toSpawn[0];
        toSpawn.RemoveAt(0);
        yield return new WaitForSeconds(entry.delay);
        for(int i = 0; i < entry.toSpawn.Length; i++)
        {
            Spawn(entry.toSpawn[i]);
        }
        if (toSpawn.Count > 0)
            StartCoroutine(spawnDelay());
        else
            Done();
    }

    void Done()
    {
        Debug.Log("done");
    }
}