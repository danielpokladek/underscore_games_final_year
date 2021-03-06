﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Player UI")]
    [Tooltip("This is the gradient that will be used to colour player's health bar, changing the currently set colours will work in game straight away. "
    + "If you'd like the colours to change quicker, move the sliders to the right, and if you'd like the colours to stay longer move them to right."
    + "Imagine the gradient slider as value between 0 and 1, and the lower is player's health the lower will be the value on the 0 to 1 scale.")]
    public Gradient healthGradient;
    public TMP_Text gemText;
    public TMP_Text soulsText;
    public TMP_Text gemCollected;
    public TMP_Text enemyKilled;

    [Header("UI Settings")]
    public GameObject gamePanel;
    public PauseMenu pausePanel;
    public InfoMenu infoPanel;
    public GameObject deathPanel;
    public Image skillOne, skillTwo, skillThree;
    public Image skillOneBack, skillTwoBack, skillThreeBack;
    public Image clockImage;
    public GameObject tutorialHud;
    public ItemPickupUI itemUI;

    [SerializeField] private PlayerController playerRef;
    private PlayerStats playerStats;

    public DialogueController dialogueController;

    private float startClockRotation;
    private float endClockRotation;
    private float clockRotation;

    private float clockRotationLength;
    private float clockRotationTimer;

    #region Singleton
    public static UIManager current;

    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        updateUICallback += UpdateUIFunction;
        updateGemsUICallback += UpdateGemsUIFunction;

        deathPanel.SetActive(false);

        LevelManager.instance.OnStateChange += LevelManager_OnStateChange;
    }

    private void LevelManager_OnStateChange(object sender, LevelManager.OnStateChangeEventArgs e)
    {
        startClockRotation = clockImage.transform.eulerAngles.z;
        endClockRotation = startClockRotation + 90.0f;

        clockRotationLength = e.stateLength;
        clockRotationTimer = 0;
    }
    #endregion

    public delegate void UpdateUI();
    public UpdateUI updateUICallback;

    public delegate void UpdateGemsUI();
    public UpdateGemsUI updateGemsUICallback;

    public void PlayerSpawned(PlayerController playerRef)
    {
        //loadingScreen.SetActive(false);
        dialogueController.gameObject.SetActive(false);

        this.playerRef = playerRef;

        if (GameManager.current.loadingFinishedCallback != null)
            GameManager.current.loadingFinishedCallback.Invoke();

        AssignImages();
    }

    public void ShowItemUI(string itemName, string itemDesc, float length)
    {
        StartCoroutine(ShowItemCoroutine(itemName, itemDesc, length));
    }

    private IEnumerator ShowItemCoroutine(string itemName, string itemDesc, float length)
    {
        itemUI.ShowUI(itemName, itemDesc);

        yield return new WaitForSeconds(length);

        itemUI.HideUI();
    }

    public void StartDialogue(Dialogue dialogue, TutorialRoom roomManager)
    {
        dialogueController.gameObject.SetActive(true);
        dialogueController.StartDialogue(dialogue);
    }

    public void EndDialogue()
    {
        dialogueController.gameObject.SetActive(false);
    }

    public void PlayerDead()
    {
        enemyKilled.text  = GameManager.current.EnemyCount.ToString("");
        gemCollected.text = GameManager.current.PlayerGems.ToString("");

        deathPanel.SetActive(true);
    }

    private void Update()
    {
        UpdateClock();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.Pause();
        }

        if (!PauseMenu.GameIsPaused)
        {
            if (Input.GetKey(KeyCode.Tab))
                infoPanel.Show();
            
            if (Input.GetKeyUp(KeyCode.Tab))
                infoPanel.Hide();
        }
    }

    private void UpdateClock()
    {
        clockRotationTimer += Time.deltaTime;

        clockRotation = Mathf.Lerp(startClockRotation, endClockRotation,
            clockRotationTimer / clockRotationLength) % 360.0f;

        clockImage.transform.eulerAngles = new Vector3(0.0f, 0.0f, clockRotation);
    }

    private void AssignImages()
    {
        playerStats = playerRef.playerStats;

        gemText.text = GameManager.current.PlayerGems.ToString("");

        skillOneBack.sprite = playerRef.skillOne;
        skillTwoBack.sprite = playerRef.skillTwoBack;
        skillThreeBack.sprite = playerRef.skillThreeBack;

        skillOne.sprite       = playerRef.skillOne;
        skillTwo.sprite       = playerRef.skillTwo;
        skillThree.sprite     = playerRef.skillThree;

        UpdateUIFunction();
        UpdateGemsUIFunction();
    }

    private void UpdateUIFunction()
    {
        skillOne.fillAmount = playerRef.GetSkillOneCooldown() / playerStats.abilityOneCooldown.GetValue();
        skillTwo.fillAmount = playerRef.GetSkillTwoCooldown() / playerStats.abilityTwoCooldown.GetValue();
        skillThree.fillAmount = playerRef.GetSkillThreeCooldown() / playerStats.abilityThreeCooldown.GetValue();
    }

    private void UpdateGemsUIFunction()
    {
        gemText.text = GameManager.current.PlayerGems.ToString("");
        soulsText.text = LevelManager.instance.enemyKills.ToString("");
    }
}