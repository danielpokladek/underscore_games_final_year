using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        if (GameIsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        UIManager.current.gamePanel.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        UIManager.current.gamePanel.SetActive(true);
    }

    public void ReturnToMenu()
    {
        GameManager.current.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex,
            (int)SceneIndexes.MENU);
    }

    public void OptionsMenu()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }
}
