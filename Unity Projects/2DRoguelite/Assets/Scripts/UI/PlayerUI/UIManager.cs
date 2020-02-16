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
    }
}
