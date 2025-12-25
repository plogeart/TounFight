using UnityEngine;
using System.Collections;

public class PortalSpawner : MonoBehaviour
{
    [Header("Configuration des Ennemis")]
    public GameObject[] enemyPrefabs;

    [Header("Temps d'Apparition")]
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    [Header("Limites")]
    public int maxEnemiesTotal = 10;
    public float spawnRadius = 3f;

    [Header("Zone de Garde des Ennemis")]
    [Tooltip("Distance max à laquelle les ennemis peuvent s'éloigner du portail")]
    public float enemyWanderLimit = 15f; 

    private int currentSpawnCount = 0;
    private bool isSpawning = true;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (isSpawning)
        {
            if (maxEnemiesTotal > 0 && currentSpawnCount >= maxEnemiesTotal)
            {
                isSpawning = false;
                yield break;
            }

            SpawnEnemy();

            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        // 1. Création
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);

        GameObject newEnemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, transform.rotation);
        
        // 2. Configuration de la zone de garde (LEASH)
        // On récupère le cerveau du nouvel ennemi
        EnemyAI ai = newEnemy.GetComponent<EnemyAI>();
        if (ai != null)
        {
            // On lui configure sa zone
            ai.SetupGuard(transform.position, enemyWanderLimit);
        }

        currentSpawnCount++;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyWanderLimit);
    }
}