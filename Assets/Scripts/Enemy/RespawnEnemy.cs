using System.Collections;
using UnityEngine;

public class RespawnEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private float respawnDelay = 0.5f;
    [SerializeField]
    private Canvas healthBar;

    private bool isRespawning = false;

    private void Update()
    {
        if (!enemy.activeSelf && !isRespawning)
        {
            healthBar.enabled = false; // Disable health bar when enemy is inactive
            StartCoroutine(RespawnCoroutine());
            isRespawning = true;
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
        isRespawning = false;
    }

    private void Respawn()
    {
        if (enemy != null)
        {
            if (enemy.TryGetComponent<EnemyHealth>(out var enemyHealth))
            {
                enemyHealth.HealFull();
            }
            else
            {
                Debug.LogWarning("Enemy does not have an EnemyHealth component.");
            }
            enemy.SetActive(true);
            enemy.transform.position = transform.position;
            healthBar.enabled = true; // Re-enable health bar when enemy respawns
        }
    }
}