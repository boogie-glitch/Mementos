using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim; // Reference to the Animator component for animations
    public static PlayerAttack instance; // Singleton instance of PlayerAttack

    public bool isAttacking = false; // Flag to check if the player is currently attacking


    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void Awake(){
        instance = this; // Set the singleton instance to this script
   }

    void Start()
    {
        anim = GetComponent<Animator>(); // Get the Animator component attached to the player
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack(){
        if(Input.GetKeyDown(KeyCode.J) && !isAttacking){
            isAttacking = true; // Set the attacking flag to true
        }
    }

    
}
