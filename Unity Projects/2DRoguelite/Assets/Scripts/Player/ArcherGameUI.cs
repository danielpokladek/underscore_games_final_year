using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArcherGameUI : MonoBehaviour
{
    [SerializeField] private ArcherCharacter playerObject;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image    healthSlider;
    [SerializeField] private Image    extraDamageSkill;
    [SerializeField] private Image    tripleShotSkill;
    [SerializeField] private Image    dodgeSkill;

    private void Start()
    {
        playerObject.onGUIUpdateCallback += UpdateUI;
    }

    private void Update()
    { 
        dodgeSkill.fillAmount       = playerObject.GetCurrentDodge  / playerObject.GetDodge;
        tripleShotSkill.fillAmount  = playerObject.GetTripleCurrent / playerObject.GetTripleCooldown;
        extraDamageSkill.fillAmount = playerObject.GetExtraCurrent  / playerObject.GetExtraCooldown;
    }

    private void UpdateUI()
    {
        healthSlider.fillAmount = playerObject.playerStats.currentHealth / playerObject.playerStats.characterHealth.GetValue();
        healthText.text         = playerObject.playerStats.currentHealth.ToString("00");
    }
}
