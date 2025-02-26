using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public EventSystem eventSystem;

    // Check the pause state of the game
    public static bool IsGamePaused()
    {
        return GameIsPaused;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f; // Resume game time
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Make the cursor invisible
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayResumeSound(); // Play resume sound
        }
        else
        {
            Debug.LogWarning("SoundManager instance is null. Cannot play resume sound.");
        }
    }

    void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        Time.timeScale = 0f; // Pause game time
        GameIsPaused = true;

        // Check if eventSystem is not null before using it
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(null);
            // Set the first selectable UI element here if needed
            // eventSystem.SetSelectedGameObject(firstSelectableElement);
        }
        else
        {
            Debug.LogWarning("EventSystem is not assigned in the PauseMenu script.");
        }
        Cursor.lockState = CursorLockMode.None; // Free the cursor
        Cursor.visible = true; // Make the cursor visible
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayPauseSound(); // Play pause sound
        }
        else
        {
            Debug.LogWarning("SoundManager instance is null. Cannot play pause sound.");
        }
    }
}
