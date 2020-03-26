using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public GameObject loadingScreen;
    public GameObject deathScreen;
    public Image playerPortrait;
    public Image healthImage;
    public Image skillOne, skillTwo, skillThree;
    public Image skillOneBack, skillTwoBack, skillThreeBack;
    public GameObject tutorialHud;

    [SerializeField] private PlayerController playerRef;
    private PlayerStats playerStats;

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

        deathScreen.SetActive(false);
    }
    #endregion

    public delegate void UpdateUI();
    public UpdateUI updateUICallback;

    public delegate void UpdateGemsUI();
    public UpdateGemsUI updateGemsUICallback;

    public void PlayerSpawned(PlayerController playerRef)
    {
        //loadingScreen.SetActive(false);

        this.playerRef = playerRef;

        if (GameManager.current.loadingFinishedCallback != null)
            GameManager.current.loadingFinishedCallback.Invoke();

        AssignImages();
    }

    public void PlayerDead()
    {
        enemyKilled.text  = GameManager.current.enemyKilled.ToString("");
        gemCollected.text = GameManager.current.gemsCollected.ToString("");

        deathScreen.SetActive(true);
    }

    private void AssignImages()
    {
        playerStats = playerRef.playerStats;

        gemText.text = GameManager.current.PlayerGems.ToString("");

        playerPortrait.sprite = playerRef.characterPortrait;

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
        //healthImage.fillAmount = playerStats.currentHealth / playerStats.characterHealth.GetValue();
        //healthImage.color = healthGradient.Evaluate(playerStats.currentHealth / playerStats.characterHealth.GetValue());

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