using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScrip : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene(1); // Chuyển sang scene 1 (scene index 1)
        Debug.Log("New game started.");
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