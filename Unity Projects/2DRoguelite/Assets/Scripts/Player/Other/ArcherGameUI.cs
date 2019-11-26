using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArcherGameUI : MonoBehaviour
{
    [SerializeField] private ArcherCharacter playerObject;

    [Header("Health")]
    [SerializeField] private Image playerHealth;
    [SerializeField] private TMP_Text playerHealthText;

    [Header("Skills")]
    [SerializeField] private Image extraDamageSkill;
    [SerializeField] private Image tripleShotSkill;
    [SerializeField] private Image dodgeSkill;

    [Header("Attack Stuff")]
    [SerializeField] private TMP_Text currentBowDraw;
    [SerializeField] private TMP_Text maxBowDraw;

    private void Start()
    {
        playerObject.onUIChangeCallback += UpdateUI;
    }

    private void Update()
    {
        //currentBowDraw.text         = playerObject.GetCurrentBowDraw.ToString("0.0");

        dodgeSkill.fillAmount       = playerObject.GetCurrentDodge  / playerObject.GetDodge;
        tripleShotSkill.fillAmount  = playerObject.GetTripleCurrent / playerObject.GetTripleCooldown;
        extraDamageSkill.fillAmount = playerObject.GetExtraCurrent  / playerObject.GetExtraCooldown;

        //playerHealth.text           = playerObject.GetCurrentHealth.ToString("000");
    }

    private void UpdateUI()
    {
        playerHealth.fillAmount = playerObject.GetCurrentHealth / playerObject.GetMaxHealth;
        playerHealthText.text   = playerObject.GetCurrentHealth.ToString("00");
    }
}
