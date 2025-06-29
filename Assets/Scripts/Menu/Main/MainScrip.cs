using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScrip : MonoBehaviour
{
    [SerializeField]
    private PlayerStatus playerStatus; // Reference to the PlayerStatus script (if needed for player status management)
    public void NewGame()
    {
        playerStatus.ResetStatus(); // Reset trạng thái người chơi nếu cần thiết
        SaveSystem.NewGame(); // Tạo một game mới và lưu trạng thái người chơi
        SceneManager.LoadScene(1); // Chuyển sang scene 1 (scene index 1)
        Debug.Log("New game started.");
    }

    public void LoadGame()
    {
        var playerData = SaveSystem.LoadPlayer(); // Tải trạng thái người chơi từ file đã lưu
        if (playerData == null)
        {
            Debug.LogError("No saved game found. Starting a new game instead.");
            return;
        }
        playerStatus.hp = playerData.health; // Cập nhật HP người chơi
        playerStatus.maxHp = playerData.maxHealth; // Cập nhật Max HP người chơi
        playerStatus.exp = playerData.experience; // Cập nhật EXP người chơi
        playerStatus.maxExp = playerData.maxExperience; // Cập nhật Max EXP người chơi
        playerStatus.level = playerData.level; // Cập nhật Level người chơi

        SceneManager.LoadScene(playerStatus.sceneName); // Chuyển sang scene đã lưu
        Debug.Log("Game loaded.");
    }

    public void QuitGame()
    {
        Application.Quit(); // Thoát game
        Debug.Log("Game has been quit.");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Thoát play mode khi test trong Editor
        #endif
    }
}