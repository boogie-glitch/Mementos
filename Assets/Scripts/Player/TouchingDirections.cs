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

    public float groundDistance = 0.05f;
    public float ceilingDistance = 0.05f;
    public float wallDistance = 0.2f;

    
    public bool IsGrounded { 
        get {
            return _isGrounded;
        } private set{
            _isGrounded = value;
            anim.SetBool(AnimationStrings.isGrounded, value);
        } 
    }

    public bool IsOnWall { 
        get {
            return _isOnWall;
        } private set{
            _isOnWall = value;
            anim.SetBool(AnimationStrings.isOnWall, value);
        } 
    }
    public bool IsOnCeiling { 
        get {
            return _isOnCeiling;
        } private set{
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
        IsGrounded = TouchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = TouchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = TouchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;

    }
}
