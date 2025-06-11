using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb; // Reference to the Rigidbody2D component for physics interactions
    public Animator anim; // Reference to the Animator component for animations

    private float movement;
    public float movespeed = 2f; // Speed of the player movement
    public bool facingRight = true; // Track the direction the player is facing


    public float jumpheight = 5f; // Height of the jump
    public bool isGround = true; // Check if the player is on the ground
    private bool doubleJump; // Allow for a double jump
    private bool isJumping; // Check if the player is currently jumping
    private bool isFalling; // Check if the player is currently falling
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        anim = GetComponent<Animator>(); // Get the Animator component attached to the player
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.K)){
            Jump(); // Call the Jump method if the space key is pressed and the player is grounded
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
            Walk(); // Call the Walk method if the A or D key is pressed
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            movespeed = 5f;
        }else {
            movespeed = 2f; // Reset movement speed when Shift is not pressed
        }

    }

    // FixedUpdate is called at a fixed interval and is independent of the frame rate
    void FixedUpdate()
    {
        
        if(Mathf.Abs(movement) > .1f){
            anim.SetFloat("Walk", 1f);
        } 
        else if(Mathf.Abs(movement) < .1f){
            anim.SetFloat("Walk", 0f);
        }
        if(Input.GetKey(KeyCode.LeftShift)) {
            anim.SetFloat("Run",1f);
        } 
        else {
            anim.SetFloat("Run",0f);
        }

    }
    void LateUpdate(){
        if(!isGround && rb.linearVelocity.y < -0.1f){
            anim.SetBool("isJumping", false); // Reset the jumping animation state if the player is not grounded
            anim.SetBool("isFalling", true); // Set the falling animation state if the player is not grounded and moving downwards
        }
        else{
            anim.SetBool("isFalling", false); // Reset the falling animation state if the player is grounded or not falling
        }
    }

    void Walk(){
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * movespeed; // Move the player left or right based on input

        if(movement < 0f && facingRight){
            transform.eulerAngles = new Vector3(0f, -180f, 0f); // Flip the player to face left
            facingRight = false; // Update the facing direction
        } 
        else if(movement > 0f && facingRight == false){
            transform.eulerAngles = new Vector3(0f, 0f, 0f); // Flip the player to face right
            facingRight = true; // Update the facing direction
        }
    }

    void Jump(){
       if(isGround){
            rb.linearVelocity= new Vector2(rb.linearVelocity.x, jumpheight); // Apply a vertical force to the player for jumping
            isGround = false; // Set isGround to false when the player jumps
            doubleJump = true; // Allow for a double jump
            anim.SetBool("isJumping", true); // Set the jump animation state
       }
       else if(doubleJump == true && !isGround) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpheight); // Apply a vertical force for the double jump
            doubleJump = false; // Disable further double jumps
            anim.SetBool("isJumping", true); // Set the jump animation state
       }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground")) // Check if the player collides with an object tagged as "Ground"
        {
            isGround = true; // Set isGround to true when the player is on the ground
            doubleJump = false; // Reset double jump ability
            anim.SetBool("isJumping", false); // Reset the jump animation state
            anim.SetBool("isFalling", false); // Reset the falling animation state
        }

    }
}
