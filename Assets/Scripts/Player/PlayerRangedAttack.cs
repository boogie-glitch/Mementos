using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField]
    private Transform arrowPoint; // Vị trí bắn projectile
    [SerializeField] 
    private GameObject[] arrows;

    private float cooldownTimer = Mathf.Infinity;
    [SerializeField]
    private float projectileSpeed; // Tốc độ của projectile

    private void Update()
    {
        
    }

    private void RangedAttack()
    {   
        cooldownTimer = 0;
        GameObject arrow = arrows[FindArrow()];
        arrow.transform.position = arrowPoint.position;
        float direction = Mathf.Sign(transform.localScale.x);

        arrow.transform.parent = null;
        arrow.GetComponent<Projectile>().SetDirection(direction);

        arrows[FindArrow()].GetComponent<Projectile>().ActivateProjectile();
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}