using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D TouchingCol;
    Animator anim;

    public ContactFilter2D castFilter;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded;
    [SerializeField]
    private bool _isOnWall;
    [SerializeField]
    private bool _isOnCeiling;

    public float groundDistance = 0.1f;
    public float ceilingDistance = 0.05f;
    public float wallDistance = 0.2f;

    
    public bool IsGrounded 
    { 
        get 
        {
            return _isGrounded;
        } 
        private set
        {
            _isGrounded = value;
            anim.SetBool(AnimationStrings.isGrounded, value);
        } 
    }

    public bool IsOnWall 
    { 
        get 
        {
            return _isOnWall;
        } 
        private set
        {
            _isOnWall = value;
            anim.SetBool(AnimationStrings.isOnWall, value);
        } 
    }
    public bool IsOnCeiling 
    { 
        get 
        {
            return _isOnCeiling;
        } 
        private set
        {
            _isOnCeiling = value;
            anim.SetBool(AnimationStrings.isOnCeiling, value);
        } 
    }

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    
    
    private void Awake()
    {
        TouchingCol = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int groundHitCount = TouchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance);
        bool foundGround = false;
        for (int i = 0; i < groundHitCount; i++)
        {
            float angle = Vector2.Angle(groundHits[i].normal, Vector2.up);
            if (angle >= 0f && angle <= 45f)
            {
                foundGround = true;
                break;
            }
        }
        IsGrounded = foundGround;

        int wallHitCount = TouchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance);
        bool foundWall = false;
        for (int i = 0; i < wallHitCount; i++)
        {
            float angle = Vector2.Angle(wallHits[i].normal, Vector2.up);
            if (angle > 50f && angle < 130f)
            {
                foundWall = true;
                break;
            }
        }

        IsOnWall = foundWall; 
        IsOnCeiling = TouchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;

    }
}
