using UnityEngine;
using System.Collections;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float respawnDelay = 10f; // Thời gian hồi sinh (giây)
    private GameObject currentEnemy;
    private Vector3 spawnPosition;


    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    private void Start()
    {
        spawnPosition = transform.position;
        SpawnEnemy();
    }
    private void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        var patrol = currentEnemy.GetComponent<EnemyPatrol>();
        if (patrol != null)
        {
            patrol.leftEdge = leftEdge;
            patrol.rightEdge = rightEdge;
        }

        var health = currentEnemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.Died.RemoveListener(OnEnemyDeath);
            health.Died.AddListener(OnEnemyDeath);
        }
    }

    private void Update()
    {
    if (Input.GetKeyDown(KeyCode.Q))
    {
        Debug.Log("Q pressed");
        Kill(); // <-- Gọi hàm Kill của EnemySpawnPoint
    }
    }
    private void OnEnemyDeath()
    {
        currentEnemy = null;
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy();
    }

    // Destroys the current enemy and triggers respawn
    private void Kill()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy); // Xóa cả cây EnemyPatrol (1)
            currentEnemy = null;
            StartCoroutine(RespawnCoroutine());
        }
    }
}