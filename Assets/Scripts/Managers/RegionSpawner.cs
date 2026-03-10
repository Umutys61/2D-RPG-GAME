using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject enemyPrefab;
        public int initialCount;
        public int maxAlive;
        public float respawnDelay;
    }

    [Header("Region Settings")]
    public List<SpawnEntry> entries = new();
    public int regionMaxAlive ;

    [Header("Spawn Points")]
    [Tooltip("Boş birakilirsa, spawner'in child objeleri spawn noktasi olur.")]
    public Transform[] spawnPoints;

    private Dictionary<SpawnEntry, List<GameObject>> alive = new();

    private void Awake()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            List<Transform> list = new();
            foreach (Transform child in transform)
                list.Add(child);
            spawnPoints = list.ToArray();
        }

        foreach (var e in entries)
            alive[e] = new List<GameObject>();
    }

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        foreach (var e in entries)
            for (int i = 0; i < e.initialCount; i++)
                SpawnOne(e);

        while (true)
        {
            foreach (var e in entries)
            {
                if (regionMaxAlive > 0 && TotalAlive() >= regionMaxAlive)
                    continue;

                if (alive[e].Count < e.maxAlive)
                    SpawnOne(e);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void SpawnOne(SpawnEntry e)
    {
        if (e.enemyPrefab == null || spawnPoints.Length == 0) return;

        Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject go = Instantiate(e.enemyPrefab, p.position, Quaternion.identity);
        alive[e].Add(go);

        var hp = go.GetComponent<EnemyHealth>();
        if (hp != null)
        {
            hp.OnDied += (enemy) =>
            {
                alive[e].Remove(go);
                StartCoroutine(RespawnLater(e));
            };
        }
    }

    private IEnumerator RespawnLater(SpawnEntry e)
    {
        yield return new WaitForSeconds(e.respawnDelay);
        SpawnOne(e);
    }

    private int TotalAlive()
    {
        int sum = 0;
        foreach (var kv in alive)
            sum += kv.Value.Count;
        return sum;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        var col = GetComponent<Collider2D>();
        if (col != null)
            Gizmos.DrawCube(col.bounds.center, col.bounds.size);

        Gizmos.color = Color.yellow;
        if (spawnPoints != null)
            foreach (var t in spawnPoints)
                if (t) Gizmos.DrawSphere(t.position, 0.1f);
    }
}
