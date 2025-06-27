using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class SceneController : MonoBehaviour
{
    public Vector3 lastPlayerPosition;

    public static SceneController instance;

    public PlayerPositionSO playerPositionSO;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PreviousLevel()
    {
        playerPositionSO.isGoBack = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        
    }
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

   
}
