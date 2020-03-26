using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip musicTheme;
    public GameObject mainMenuPanel;
    public GameObject startPanel;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        startPanel.SetActive(false);

        AudioManager.current.PlayMusic(musicTheme);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            BTN_PlayGame();

        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.F1))
                BTN_TestScene();
        }
    }

    public void BTN_StartGame()
    {
        mainMenuPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void BTN_PlayTutorial()
    {
        GameManager.current.LoadTutorial();
    }

    public void BTN_PlayGame()
    {
        GameManager.current.LoadMain();
    }

    public void BTN_TestScene()
    {
        GameManager.current.LoadScene((int)SceneIndexes.TITLE_SCREEN, (int)SceneIndexes.TEST);
    }

    public void BTN_Quit()
    {
        Application.Quit();
    }
}
