using UnityEngine;

public class PlayerAttackAir : MonoBehaviour
{
    public static PlayerAttackAir instance; // Singleton instance of PlayerAttackAir
    public Animator anim; // Reference to the Animator component for animations

    public bool isAttackingAir = false; // Flag to check if the player is currently attacking in the air
    
    private void Awake()
    {
        instance = this; // Set the singleton instance to this script
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>(); // Get the Animator component attached to the player    
    }

    // Update is called once per frame
    void Update()
    {
        AttackAir(); // Call the Attack method to check for attack input
    }

    void AttackAir()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isAttackingAir && Player.instance.isGround == false)
        {
            isAttackingAir = true; // Set the attacking flag to true
        }
    }

}
