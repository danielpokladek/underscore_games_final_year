using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InfoMenu : MonoBehaviour
{
    [SerializeField] private GameObject infoMenu;

    [SerializeField] private TMP_Text levelNo;
    [SerializeField] private TMP_Text gemsNo;
    [SerializeField] private TMP_Text soulsNo;
    [SerializeField] private TMP_Text gameTime;
    [SerializeField] private TMP_Text gameScore;

    private bool ShowingInfo = false;

    private void Start()
    {
        infoMenu.SetActive(false);
    }

    public void ShowInfo()
    {
        if (ShowingInfo)
            Hide();
        else
            Show();
    }

    public void Hide()
    {
        UIManager.current.gamePanel.SetActive(true);

        infoMenu.SetActive(false);
        ShowingInfo = false;
    }

    public void Show()
    {
        UpdateUI();

        UIManager.current.gamePanel.SetActive(false);

        infoMenu.SetActive(true);
        ShowingInfo = true;
    }

    private void UpdateUI()
    {
        gemsNo.text = GameManager.current.PlayerGems.ToString("");
        soulsNo.text = LevelManager.instance.enemyKills.ToString("");
        levelNo.text = GameManager.current.LevelCount.ToString("");

        var temp = TimeSpan.FromSeconds(GameManager.current.GameTime);

        gameTime.text = string.Format("{0:00} : {1:00} : {2:00}", temp.Hours, temp.Minutes, temp.Seconds);
        gameScore.text = GameManager.current.GameScore.ToString("");
    }
}
