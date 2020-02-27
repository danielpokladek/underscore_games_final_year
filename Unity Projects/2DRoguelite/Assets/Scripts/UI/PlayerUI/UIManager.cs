using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject loadingScreen;
    public Image playerPortrait;
    public Image healthImage;
    [Tooltip("This is the gradient that will be used to colour player's health bar, changing the currently set colours will work in game straight away. "
    + "If you'd like the colours to change quicker, move the sliders to the right, and if you'd like the colours to stay longer move them to right."
    + "Imagine the gradient slider as value between 0 and 1, and the lower is player's health the lower will be the value on the 0 to 1 scale.")]
    public Gradient healthGradient;
    public Image skillOne, skillTwo, skillThree;

    private PlayerController playerRef;
    private PlayerStats playerStats;

    #region Singleton
    public static UIManager current;

    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);
    }
    #endregion

    public void PlayerSpawned()
    {
        //loadingScreen.SetActive(false);

        if (GameManager.current.loadingFinishedCallback != null)
            GameManager.current.loadingFinishedCallback.Invoke();
    }

    private void Start()
    {
        GameManager.current.loadingFinishedCallback += AssignImages;
    }

    private void AssignImages()
    {
        playerRef                     = GameManager.current.playerRef.GetComponent<PlayerController>();
        playerStats                   = playerRef.playerStats;
        playerRef.onUIUpdateCallback += UpdateUI;

        playerPortrait.sprite = playerRef.characterPortrait;
        skillOne.sprite       = playerRef.skillOne;
        skillTwo.sprite       = playerRef.skillTwo;
        skillThree.sprite     = playerRef.skillThree;

        UpdateUI();
    }

    private void UpdateUI()
    {
        healthImage.fillAmount = playerStats.currentHealth / playerStats.characterHealth.GetValue();

        healthImage.color = healthGradient.Evaluate(playerStats.currentHealth / playerStats.characterHealth.GetValue());
    }
}