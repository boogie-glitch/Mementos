using UnityEngine;

public class PreviousPoint : MonoBehaviour
{
    [SerializeField] private PlayerPositionSO playerPositionSO;
    [SerializeField] private GameObject transformation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPositionSO.lastPlayerPosition = (transformation.transform.position - transform.position) / transform.localScale.x;
            SceneController.instance.PreviousLevel();
        }
    }
}
