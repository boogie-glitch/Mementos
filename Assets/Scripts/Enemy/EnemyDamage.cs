using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // [SerializeField] protected float damage;

    // protected void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.tag == "Player")
    //         collision.GetComponent<PlayerHealth>().TakeDamage(damage);
    // }
    public void OnTriggerEnter2D(Collider2D other)
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
            var animator = other.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Damaged");
            }
        }
    }

}