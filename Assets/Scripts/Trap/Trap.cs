using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (other.TryGetComponent<PlayerHealth>(out var health))
        {
            if (other.TryGetComponent<Healthbar>(out var healthbar))
            {
                healthbar.SetValue(health.Hp - 10); // Assuming the trap deals 10 damage
            }

            health.Damage(10);
            var animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Damaged");
            }
        }
    }
}
