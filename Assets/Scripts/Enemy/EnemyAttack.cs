using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth playerHealth; // Reference to the player's health script
    [SerializeField]
    private Healthbar healthBar; // Reference to the player's health bar

    public void DamagePlayer()
    {
        // if (PlayerInSight())
        // // playerHealth.TakeDamage(damage);
        // {
            healthBar.SetValue(playerHealth.Hp - 10); // Assuming the trap deals 10 damage
            playerHealth.Damage(10);
            var animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Damaged");
            }
        // }

    }
}
