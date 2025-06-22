using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private int damageAmount; // Amount of damage dealt by the trap

    // This method is called when the player collides with an enemy
    // It assumes that the enemy has a Health component and a Healthbar component
   protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        if (other.TryGetComponent<EnemyHealth>(out var health))
        {
            if (other.TryGetComponent<Healthbar>(out var healthbar))
            {
                healthbar.SetValue(health.Hp - damageAmount); // Assuming the trap deals 10 damage
            }

            health.Damage(damageAmount);
            var animator = other.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Damaged");
            }
        }
    }
}
