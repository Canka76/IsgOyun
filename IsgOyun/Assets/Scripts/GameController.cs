using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject pause_menu;
    public GameObject winner_menu;
    public TMP_Text winner_number;
    private bool isGameOver = false;
    private bool isPaused = false;

    public void FromPauseMenu()
    {
        pause_menu.active = false;
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("kosor");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause_menu.SetActive(!pause_menu.activeInHierarchy);
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
        }

        if (isGameOver)
        {
            winner_menu.active = true;

            //winner_number = P1_point > P2_point ? "1" : "2";
        }
    }
}
