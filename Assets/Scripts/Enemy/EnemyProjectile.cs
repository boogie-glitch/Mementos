using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private Transform parent;
    private float lifetime;
    private Animator anim;
    private BoxCollider2D coll;


    private bool hit;
    private float moveDirection = 1f;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }
    private void Update()
    {
        if (hit)
        {
            return;
        }

        float movementoSpeed = speed * Time.deltaTime;
        transform.Translate(Vector2.right * moveDirection * movementoSpeed);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
        {
            Deactivate(); // Deactivate the projectile after a certain time
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision); //Execute logic from parent script first
        coll.enabled = false;

        if (anim != null)
        {
            anim.SetTrigger("explode"); //When the object is a fireball explode it
        }
        else
        {
            Deactivate(); //When this hits any object deactivate arrow
        }
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
        SetParent();
    }


    public void SetDirection(float direction)
    {
        moveDirection = direction;
        // Nếu muốn lật sprite, có thể lật scale.x ở đây
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    public void SetParent()
    {
        gameObject.transform.parent = parent;
    }
}