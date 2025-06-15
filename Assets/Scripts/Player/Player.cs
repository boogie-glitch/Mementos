using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb; // Reference to the Rigidbody2D component for physics interactions
    [SerializeField] private Animator anim; // Reference to the Animator component for animations
    [SerializeField] private TrailRenderer trailRenderer; // Reference to the TrailRenderer component for dash effect

    public static Player instance; // Singleton instance of the Player class

    private float movement;
    public float movespeed = 3f; // Speed of the player movement
    public bool facingRight = true; // Track the direction the player is facing


    public float jumpheight = 10f; // Height of the jump
    public bool isGround = true; // Check if the player is on the ground
    private bool doubleJump; // Allow for a double jump
    private bool isJumping; // Check if the player is currently jumping
    private bool isFalling; // Check if the player is currently falling
    
    //Dash
    private bool isDashing = false; // Check if the player is currently dashing
    private bool canDash = true; // Check if the player can dash
    private float dashSpeed = 7f; // Speed of the dash
    private float dashTime = 0.4f; // Duration of the dash
    private float dashCooldown = 1f; // Cooldown time after dashing


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        anim = GetComponent<Animator>(); // Get the Animator component attached to the player

    }

    private void Awake()
    {
        instance = this; // Set the singleton instance to this script
    }

    // Update is called once per frame
    void Update()
    {
        if(isDashing) return; // If the player is currently dashing, skip the rest of the Update method


        movement = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.K)){
            Jump(); // Call the Jump method if the space key is pressed and the player is grounded
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
            Walk(); // Call the Walk method if the A or D key is pressed
            if(Input.GetKey(KeyCode.LeftShift)){
                movespeed = 6f;
            }
            else {
                movespeed = 3f; // Reset movement speed when Shift is not pressed
            }
        }
        if(Input.GetKeyDown(KeyCode.L) && canDash) {
            StartCoroutine(Dash()); // Call the Dash method if the L key is pressed and the player is not currently dashing
        }
        
    }

    // FixedUpdate is called at a fixed interval and is independent of the frame rate
    void FixedUpdate()
    {
        if(isDashing) return; // If the player is currently dashing, skip the rest of the FixedUpdate method

        if(Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(movement) > .1f){
            anim.SetFloat("Run", 1f);
            anim.SetFloat("Walk", 0f); // Set the walk animation to 0 when running
        } 
        else if(Mathf.Abs(movement) > .1f){
            anim.SetFloat("Walk",1f);
            anim.SetFloat("Run", 0f); // Set the run animation to 0 when walking
        } 
        else {
            anim.SetFloat("Run",0f);
            anim.SetFloat("Walk", 0f);
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
        //transform.position += new Vector3(movement, 0f, 0f) * movespeed * Time.deltaTime;
        rb.linearVelocity = new Vector2(movement * movespeed, rb.linearVelocity.y); // Apply horizontal movement to the player

        if (movement < 0f && facingRight){
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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpheight); // Apply a vertical force to the player for jumping
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

    private IEnumerator Dash()
    {
        canDash = false; // Disable further dashing
        isDashing = true; // Set isDashing to true to indicate the player is currently dashing
        anim.SetBool("isDashing", true); // Set the dashing animation state
        anim.SetBool("isJumping", false); // Reset the jumping animation state
        anim.SetBool("isFalling", false); // Reset the falling animation state
        float originalGravity = rb.gravityScale; // Store the original gravity scale
        rb.gravityScale = 0; // Disable gravity during the dash
        rb.linearVelocity = new Vector2(facingRight ? dashSpeed : -dashSpeed, 0); // Apply a horizontal force for the dash
        trailRenderer.emitting = true; // Enable the trail renderer to show the dash effect
        yield return new WaitForSeconds(dashTime); // Wait for the duration of the dash
        rb.linearVelocity = Vector2.zero; // Stop the player's movement after the dash
        trailRenderer.emitting = false; // Disable the trail renderer after the dash
        rb.gravityScale = originalGravity; // Restore the original gravity scale
        isDashing = false; // Set isDashing to false to indicate the player has finished dashing
        anim.SetBool("isDashing", false); // Reset the dashing animation state
        yield return new WaitForSeconds(dashCooldown); // Wait for the cooldown period before allowing another dash
        canDash = true; // Re-enable dashing after the cooldown
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
