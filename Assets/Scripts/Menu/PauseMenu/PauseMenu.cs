using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] 
    private GameObject pauseMenuUI; // Reference to the pause menu UI GameObject
    public static bool GameIsPaused = false; // Static variable to track if the game is paused
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenuUI.SetActive(false); // Ensure the pause menu is hidden at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Check if the Escape key is pressed
        {
            if (GameIsPaused) // If the pause menu is currently active
            {
                ResumeGame(); // Resume the game
            }
            else
            {
                PauseGame(); // Pause the game
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        GameIsPaused = true; // Set the static variable to true indicating the game is paused
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        GameIsPaused = false; // Set the static variable to false indicating the game is not paused
    }

    public void ReturnMainMenu()
    {
        // Load the main menu scene (assuming the main menu scene is named "MainMenu")
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f; // Ensure time scale is reset when returning to the main menu
        GameIsPaused = false; // Reset the pause state
    }
    public void QuitGame()
    {
        Application.Quit(); // Quit the application
        Debug.Log("Game is quitting..."); // Log a message to the console
    }
}
