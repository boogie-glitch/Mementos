using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    //[SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    // [SerializeField] private float colliderDistance;
    //[SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //References
    [SerializeField]
    private Animator anim;
    // private Health playerHealth;
    private PlayerHealth playerHealth;
    [SerializeField]
    private EnemyPatrol enemyPatrol;

    private Healthbar healthBar;
    private bool playerInSight = false;


    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        

        if (enemyPatrol != null)
            enemyPatrol.enabled = !playerInSight;
    }



    private System.Collections.IEnumerator AttackCooldownRoutine()
    {
        //cooldownTimer = 0;
        yield return new WaitForSeconds(0.5f); // Wait for the attack animation to finish
        playerInSight = false;
        yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown period
        //cooldownTimer = Mathf.Infinity; // Reset cooldown timer
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            playerInSight = false;
            return;
        }

        if (other.TryGetComponent<PlayerHealth>(out playerHealth))
        {
            var canvas = other.GetComponentInChildren<Canvas>();
            healthBar = canvas.GetComponentInChildren<Healthbar>();
            if (healthBar)
            {
                playerInSight = true;
                //Debug.Log("Player detected, triggering melee attack.");
                // TriggerAttack(health, healthbar); // Trigger the melee attack animation
                // if (cooldownTimer >= attackCooldown)
                // {
                //     cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
                // }
                StartCoroutine(AttackCooldownRoutine());
            }
            else
            {
                Debug.LogWarning("Healthbar component not found on Player.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerHealth component not found on Player.");
        }

        

    }
}