using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject normalEnemyPrefab;   // 10 pts, slower
    public GameObject fastEnemyPrefab;     // 25 pts, faster

    [Header("Spawn Settings")]
    public float initialSpawnInterval = 1.5f;
    public float minimumSpawnInterval = 0.3f;  // never goes below this
    public float difficultyRate = 0.05f;        // interval reduction per second

    [Header("Bounds")]
    public float spawnY = 6f;
    public float xMin = -8.5f;
    public float xMax = 8.5f;

    private float spawnInterval;
    private float spawnTimer;
    private bool active = true;

    void Start()
    {
        spawnInterval = initialSpawnInterval;
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        if (!active) return;

        // Increase difficulty over time
        spawnInterval -= difficultyRate * Time.deltaTime;
        spawnInterval = Mathf.Max(spawnInterval, minimumSpawnInterval);

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        float x = Random.Range(xMin, xMax);
        Vector3 pos = new Vector3(x, spawnY, 0f);

        // 30% chance to spawn fast enemy
        GameObject prefab = (Random.value < 0.3f) ? fastEnemyPrefab : normalEnemyPrefab;
        Instantiate(prefab, pos, Quaternion.identity);
    }

    public void StopSpawning()
    {
        active = false;
    }
}