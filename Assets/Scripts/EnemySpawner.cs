using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject normalEnemyPrefab;
    public GameObject fastEnemyPrefab;

    [Header("Warning")]
    public GameObject warningIndicatorPrefab;

    [Header("Spawn Settings")]
    public float initialSpawnInterval = 1.5f;
    public float minimumSpawnInterval = 0.3f;
    public float difficultyRate = 0.05f;

    [Header("Bounds")]
    public float spawnY = 16f;
    public float xMin = -8f;
    public float xMax = 8f;

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
        StartCoroutine(SpawnWithWarning(pos));
    }

    System.Collections.IEnumerator SpawnWithWarning(Vector3 pos)
    {
        if (warningIndicatorPrefab != null)
        {
            GameObject warning = Instantiate(warningIndicatorPrefab,
                pos, Quaternion.identity);
            Destroy(warning, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return null;
        }

        GameObject prefab = (Random.value < 0.3f) ? fastEnemyPrefab : normalEnemyPrefab;
        Instantiate(prefab, pos, Quaternion.identity);
    }

    public void StopSpawning()
    {
        active = false;
    }
}