using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(targetPathGenerator), typeof(TileMapBuilder))]
public class TargetSpawner : MonoBehaviour
{
    public static TargetSpawner instance;
    public static TargetSpawner getInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public struct SpawnEntry
    {
        public bool isEndOfWave;
        public float delay;
        public targerScriptableOBject[] toSpawn;
    }

    public List<SpawnEntry> toSpawn;

    public GameObject targetPrefab;
    public Transform parrent;

    TileMapBuilder tmb;
    targetPathGenerator path;

    private void Start()
    {
        tmb = GetComponent<TileMapBuilder>();
        path = GetComponent<targetPathGenerator>();

    }

    public void startSpawn()
    {
        if (toSpawn.Count > 0)
            StartCoroutine(spawnDelay());
    }

    public int getWaveCount()
    {
        int temp = 0;
        for (int i = 0; i < toSpawn.Count; i++)
            if (toSpawn[i].isEndOfWave)
                temp++;
        return temp;
    }

    void Spawn(targerScriptableOBject toSpawn)
    {
        Transform temp = Instantiate(targetPrefab).transform;
        temp.parent = parrent;
        temp.localScale = Vector3.one;
        temp.position = tmb.tm.CellToWorld(TileMapBuilder.mapToTile(path.lowerStart)) + new Vector3(0, 0.25f, 0);

        standartTarget target = temp.GetComponent<standartTarget>();
        target.prefab = toSpawn;
        target.layer = 1;
        target.from = path.lowerStart;
        target.to = path.getNextTarget(path.lowerStart, 1);
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
        if (toSpawn.Count > 0 && !entry.isEndOfWave)
            StartCoroutine(spawnDelay());
    }
}