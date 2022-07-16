using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(targetPathGenerator), typeof(TileMapBuilder))]
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

    TileMapBuilder tmb;
    targetPathGenerator path;

    private void Start()
    {
        tmb = GetComponent<TileMapBuilder>();
        path = GetComponent<targetPathGenerator>();

        if (toSpawn.Count > 0)
            StartCoroutine(spawnDelay());
        else
            Done();
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