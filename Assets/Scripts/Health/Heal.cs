using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Heal : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    private Image[] healIcons;
    [Range(1, 100)]
    public int healAmount = 35; // Heal amount per use
    private int healUsesLeft = 3;

    [SerializeField]
    private float healCooldown = 1f; // Cooldown time in seconds
    [SerializeField]
    private float healWaiting = 2f; // Waiting time before the next heal can be used
    [SerializeField]
    private bool canUseHeal = true; // Whether the player can use heal

    private void Start()
    {
        UpdateHealIcons();
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (healUsesLeft == 0)
        {
            Debug.Log("No heal uses left.");
            return; // No uses left, do nothing
        }

        if (!context.started)
        {
            Debug.Log("Heal action not started.");
            return; // Only trigger on action start
        }

        if (!canUseHeal)
        {
            Debug.Log("Heal is on cooldown or not allowed.");
            return; // Heal is on cooldown or not allowed
        }

        if (playerHealth.Hp == playerHealth.MaxHp)
        {
            Debug.Log("Player is already at full health.");
            return; // Player is already at full health
        }

        StartCoroutine(HealCooldownRoutine());
    }

    private void UpdateHealIcons()
    {
        for (int i = 0; i < healIcons.Length; i++)
        {
            healIcons[i].enabled = i < healUsesLeft;
        }
    }
    private System.Collections.IEnumerator HealCooldownRoutine()
    {
        canUseHeal = false;
        yield return new WaitForSeconds(healWaiting);

        playerHealth.Heal(healAmount);
        healUsesLeft--;

        UpdateHealIcons();

        EnableHeal(); // Re-enable heal input if there are uses left

        //yield return new WaitForSeconds(healCooldown);
        canUseHeal = true; // Allow heal to be used again after cooldown
    }

    private void EnableHeal()
    {
        if (healUsesLeft <= 0)
        {
            // Disable heal input if no uses left
            var playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.actions["Heal"].Disable();
            }
        }
        else
        {
            // Re-enable heal input if there are uses left
            var playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.actions["Heal"].Enable();
            }
        }
    }
}
