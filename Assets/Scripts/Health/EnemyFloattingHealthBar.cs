using UnityEngine;

public class EnemyFloattingHealthBar : MonoBehaviour
{
    [SerializeField]
    private Transform target; // The target enemy to follow
    [SerializeField]
    private Camera mainCamera; // The main camera to adjust the health bar's position
    [SerializeField]
    private Canvas canvas; // The canvas to which the health bar belongs

    private void Update()
    {
        transform.rotation = mainCamera.transform.rotation; // Keep the health bar facing the camera
        Vector3 targetPosition = target.position; // Get the target's position
        transform.position = new Vector3(targetPosition.x, targetPosition.y + 0.5f, targetPosition.z); // Adjust the height to float above the enemy

        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x); 
        transform.localScale = localScale;
    }
}
