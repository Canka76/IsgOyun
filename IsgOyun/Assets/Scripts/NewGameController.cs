using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameController : MonoBehaviour
{
    public GameObject pause_menu;
    public GameObject winner_menu;
    public TMP_Text winner_number;
    public GameObject uiMenu;
    private bool isGameOver = false;
    private bool isPaused = false;

    public void FromPauseMenu()
    {

        // Enable/Disable UI elements correctly
        pause_menu.SetActive(!isPaused);
        uiMenu.SetActive(!isPaused);

        // Freeze/unfreeze time
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Reset timescale before reloading scene
        SceneManager.LoadScene("Oyun");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            FromPauseMenu(); // Use the existing method to handle pause
        }

        if (isGameOver)
        {
            winner_menu.SetActive(true);
        }
    }

    // Call this method when the game ends (connect to your scoring system)
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0; // Freeze game
    }
}