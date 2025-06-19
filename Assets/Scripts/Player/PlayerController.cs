using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D) ,typeof(TouchingDirections))]

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator anim;
    TouchingDirections touchingDirections;
    TrailRenderer tr;

    public static PlayerController Instance ;
    // Input action for player movement


    Vector2 moveInput;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 7f;
    public float dashSpeed = 1f;


    private bool _isMoveing = false;
    private bool _isRunning = false;
    private bool _isFacingRignt = true;
    private bool _isDashing = false;
    public bool isAttacking = false;


    [SerializeField] private float dashTime = 0.2f;
    private float dashCooldown = 0.4f;
    private bool canDash = true;
    private bool canDoubleJump = true;
    private bool canAirDash = true;



    public float CurrentMoveSpeed 
    { 
        get
        {
            if(CanMove)
            {
                if (!(IsMoveing && !touchingDirections.IsOnWall)) 
                {
                    return 0f;
                }
            
                if(IsRunning)
                {
                    return runSpeed;
                } 
                return walkSpeed;    
                }
            else
            {
                return 0f;
            }
                 
        }
    }

    [SerializeField]
    public bool IsMoveing 
    { 
        get
        {
            return _isMoveing; 
        } 
        private set 
        {
            _isMoveing = value;
            anim.SetBool(AnimationStrings.isMoveing, value);
        } 
    }

    [SerializeField]
    public bool IsRunning 
    { 
        get 
        {
            return _isRunning; 
        } 
        private set 
        {
            _isRunning = value;
            anim.SetBool(AnimationStrings.isRunning, value);
        } 
    }

    [SerializeField]
    public bool IsFacingRignt 
    { 
        get 
        {
            return _isFacingRignt; 
        } 
        private set 
        {
            if (_isFacingRignt != value)
            {
               transform.localScale *= new Vector2(-1,1);
            }
            _isFacingRignt = value;
        } 
    }

    [SerializeField]
    public bool IsDashing 
    { 
        get 
        {
            return _isDashing; 
        } 
        private set 
        {
            _isDashing = value;
            anim.SetBool(AnimationStrings.isDashing, value);
        } 
    }

    [SerializeField]
    public bool CanMove{
        get
        {
            return anim.GetBool(AnimationStrings.canMove);
        }
    }

    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        tr = GetComponent<TrailRenderer>();
        Instance = this;
    }

    void Update()
    {
        if(touchingDirections.IsGrounded)
        {
            canAirDash = true; // Reset air dash when grounded
        }

        
        
    }

    void FixedUpdate()
    {
        
        if (!IsDashing)
        {
            rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
        }
        anim.SetFloat(AnimationStrings.yVelocity, rb.linearVelocity.y);  

        
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        // Handle player movement input
        moveInput = context.ReadValue<Vector2>();
        
        IsMoveing = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);

    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (IsDashing)
        {
            return;
        } 
        if(CanMove)
        {
            if (moveInput.x > 0 && !IsFacingRignt)
            {
                IsFacingRignt = true;
            }
            else if (moveInput.x < 0 && IsFacingRignt)
            {
                IsFacingRignt = false;
            }
        }
        
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        // Handle player running input
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Handle player jump input
        if(context.started && touchingDirections.IsGrounded && !IsDashing && CanMove)
        {
            canDoubleJump = true; // Reset double jump when grounded
            anim.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if(canDoubleJump && context.started && !touchingDirections.IsGrounded && !IsDashing)
        {
            // anim.SetTrigger(AnimationStrings.jump);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = false;
        }

    }   

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!canAirDash)
        {
            return;
        }
        
        if (context.started && canDash && !IsDashing)
        {
            StartCoroutine(DashCoroutine());  
        }        
    }

    private IEnumerator DashCoroutine()
    {
        if (!touchingDirections.IsGrounded)
        {
            canAirDash = false; // Disable air dash after dashing
        }
        canDash = false;
        IsDashing = true;

        float originalGravityScale = rb.gravityScale;

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0f); // SỬA Ở ĐÂY
        tr.emitting = true; 

        yield return new WaitForSeconds(dashTime);
        IsDashing = false;
        
        tr.emitting = false;
        rb.gravityScale = originalGravityScale;

        
        SetFacingDirection(moveInput);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!isAttacking)
            {
                isAttacking = true;
            }

        }
    }
}

