using UnityEngine;

public class PlayerComboAttack : MonoBehaviour
{
    public Collider2D[] attackColliders; // Gắn 3 collider cho 3 đòn

    public void EnableAttackCollider(int index)
    {
        attackColliders[index].enabled = true;
    }

    public void DisableAllColliders(int index)
    {
        attackColliders[index].enabled = false;

    }
}
