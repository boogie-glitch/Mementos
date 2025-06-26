using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D) ,typeof(TouchingDirections))]

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator anim;
    TouchingDirections touchingDirections;
    TrailRenderer tr;

    public static PlayerController Instance;
    // Input action for player movement
    [SerializeField]


    public Vector2 moveInput;
    private float attackMoveSpeed = 0.5f;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 7f;
    public float dashSpeed = 1f;
    private float knowckTime = 0f; // Duration of knockback effect



    private bool _isMoveing = false;
    private bool _isRunning = false;
    private bool _isFacingRignt = true;
    private bool _isDashing = false;
    public bool isAttacking = false;
    public bool isMoveAttack = false;
    public bool canTurn = true; // Allow turning while moving


    [SerializeField] private float dashTime = 0.2f;
    private float dashCooldown = 0.4f;
    private bool canDash = true;
    private bool canDoubleJump = true;
    private bool canAirDash = true;

   

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (!(IsMoveing && !touchingDirections.IsOnWall))
                {
                    return 0f;
                }
                if (isMoveAttack)
                {
                    return attackMoveSpeed;
                }
                if (IsRunning)
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
        if (touchingDirections.IsGrounded)
        {
            canAirDash = true; // Reset air dash when grounded
        }

        if (knowckTime > 0)
        {
            knowckTime -= Time.deltaTime;
            if (knowckTime < 0f) knowckTime = 0f; // Đảm bảo không bị âm
        }
        //if (isMoveAttack)
        //{
        //    // Simulate the "Performed" phase by directly calling OnMove with the current moveInput
        //    var simulatedContext = new InputAction.CallbackContext();
        //    moveInput = simulatedContext.ReadValue<Vector2>();
        //    OnMove(simulatedContext);
           
        //}

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
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        if (anim.GetBool(AnimationStrings.isRangeAttack))
        {
            //moveInput = Vector2.zero; // Stop movement while attacking
            IsMoveing = false;
            return;
        }
        if (anim.GetBool(AnimationStrings.isAttacking))
        {
            isMoveAttack = true;
        }
        // Handle player movement input
        moveInput = context.ReadValue<Vector2>();
        
        IsMoveing = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);

    }

    public void SetFacingDirection(Vector2 moveInput)
    {
        if (IsDashing)
        {
            return;
        }
        if (!canTurn)
        {
            return; // Do not change direction if canTurn is false
        }
        if (CanMove)
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
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        if (anim.GetBool(AnimationStrings.isAttacking) || anim.GetBool(AnimationStrings.isRangeAttack))
        {
            IsRunning = false; // Stop running while attacking
            return;
        }
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
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        if (IsDashing)
        {
            return; // Stop jumping while attacking or dashing
        }
        if (anim.GetBool(AnimationStrings.isAttacking) || anim.GetBool(AnimationStrings.isRangeAttack)) 
        {
            return;
        }
        // Handle player jump input
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            canDoubleJump = true; // Reset double jump when grounded
            anim.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if (canDoubleJump && context.started && !touchingDirections.IsGrounded)
        {
            // anim.SetTrigger(AnimationStrings.jump);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = false;
        }

    }   

    public void OnDash(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        if (anim.GetBool(AnimationStrings.isAttacking) || IsDashing || anim.GetBool(AnimationStrings.isRangeAttack))
        {
            return; // Stop dashing while attacking or already dashing
        }
        if (!canAirDash)
        {
            return;
        }
        
        if (context.started && canDash)
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
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        if (IsDashing || !touchingDirections.IsGrounded || anim.GetBool(AnimationStrings.isRangeAttack) || !context.started)
        {
            return; // Stop attacking while dashing or in the air
        }
        if (anim.GetBool(AnimationStrings.isAttacking))
        {
            return;
        }
        if (IsMoveing)
        {
            //IsMoveing = false; // Stop moving while attacking
            isMoveAttack = true; // Set move attack flag
        }
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetBool(AnimationStrings.isAttacking, true);
        }
    }

    public void OnRangeAttack(InputAction.CallbackContext context)
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        if (!context.started || anim.GetBool(AnimationStrings.isAttacking) || !touchingDirections.IsGrounded || IsDashing)
        {
            return;
        }
        anim.SetTrigger(AnimationStrings.rangeAttack);
        anim.SetBool(AnimationStrings.isRangeAttack, true);
    }

    public void OnKnockback(Vector2 force, float duration)
    {
       if (knowckTime > 0f)
       {
            return; // Đang cooldown, không nhận knockback
       }

    knowckTime = 1f; // 1 giây cooldown
    StartCoroutine(KnockbackCoroutine(force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 force, float duration)
    {
        IsMoveing = false;
        IsRunning = false;
        IsDashing = false;

        rb.linearVelocity = Vector2.zero; // Reset velocity
        rb.AddForce(force, ForceMode2D.Impulse); // Apply knockback force
        // rb.gravityScale = 0f; // Disable gravity during knockback

        yield return new WaitForSeconds(0.1f); // Adjust the duration as needed

        // rb.gravityScale = 1f; // Re-enable gravity after knockback

        yield return new WaitForSeconds(duration - 0.1f); // Wait for the remaining duration
        rb.linearVelocity = Vector2.zero; // Reset velocity after knockback
        IsMoveing = true; // Re-enable movement after knockback
        IsRunning = false; // Reset running state
        IsDashing = false; // Reset dashing state

        anim.SetBool(AnimationStrings.isAttacking, false); // Reset attacking state
        isAttacking = false; // Reset attacking state

        anim.SetBool(AnimationStrings.isRangeAttack, false); // Reset ranged attack state
        anim.SetBool(AnimationStrings.canMove, true); // Re-enable movement
    
    }
}

