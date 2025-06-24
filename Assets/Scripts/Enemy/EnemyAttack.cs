using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Collider2D attackCollider; // The collider that represents the attack area

    public void EnableAttackCollider()
    {
        attackCollider.enabled = true; // Enable the attack collider
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false; // Disable the attack collider
    }
}
