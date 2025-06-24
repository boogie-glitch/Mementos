using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;
    [SerializeField, Range(0, 100)] 
    private int damage; // Assuming the trap deals 10 damage
    [SerializeField]
    private Transform parent;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (hit)
        {
            return;
        } 
        
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5)
        {
            //gameObject.SetActive(false);
            Deactivate();
        }
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        boxCollider.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("Enemy") || collision.CompareTag("Fireball")))
        {
            return;
        }

        hit = true;
        // anim.SetTrigger("Hits");

        if (collision.TryGetComponent<EnemyHealth>(out var health))
        {
            if (collision.TryGetComponent<EnemyHealthBar>(out var healthbar))
            {
                healthbar.SetValue(health.Hp - damage); // Assuming the trap deals 10 damage
            }

            health.Damage(damage);
            var animator = collision.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Damaged");
            }
        }
        Deactivate();
    }
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        // gameObject.SetActive(true);
        hit = false;
        // boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        SetParent();
    }

    public void SetParent()
    {
        gameObject.transform.parent = parent;
    }
}