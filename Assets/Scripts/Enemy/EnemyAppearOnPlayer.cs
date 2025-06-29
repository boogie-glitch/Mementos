using UnityEngine;

public class EnemyAppearOnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject enemyObject; // Gán enemy cần ẩn/hiện
    public GameObject playerObject; // Biến lưu player

    private void Start()
    {
        enemyObject.SetActive(false); // Ẩn enemy lúc đầu
        playerObject = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyObject.SetActive(true); // Hiện enemy khi player vào vùng
            playerObject = other.gameObject; // Lưu lại player
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyObject.SetActive(false); // Ẩn enemy khi player rời vùng
            playerObject = null; // Xóa tham chiếu player
        }
    }
}