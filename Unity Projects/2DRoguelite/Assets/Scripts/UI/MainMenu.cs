using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject startPanel;

    private void Start()
    {
        mainMenuPanel.SetActive(true);

        startPanel.SetActive(false);
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
}
