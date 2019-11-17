using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArcherGameUI : MonoBehaviour
{
    [SerializeField] private ArcherCharacter playerObject;
    [SerializeField] private TMP_Text playerHealth;
    [SerializeField] private Image extraDamageSkill;
    [SerializeField] private Image tripleShotSkill;
    [SerializeField] private Image dodgeSkill;
    [SerializeField] private TMP_Text currentBowDraw;
    [SerializeField] private TMP_Text maxBowDraw;

    private void Start()
    {
        //maxBowDraw.text = playerObject.GetBowDraw.ToString("0.0");
    }

    private void Update()
    {
        //currentBowDraw.text         = playerObject.GetCurrentBowDraw.ToString("0.0");

        dodgeSkill.fillAmount       = playerObject.GetCurrentDodge  / playerObject.GetDodge;
        tripleShotSkill.fillAmount  = playerObject.GetTripleCurrent / playerObject.GetTripleCooldown;
        extraDamageSkill.fillAmount = playerObject.GetExtraCurrent  / playerObject.GetExtraCooldown;

        //playerHealth.text           = playerObject.GetCurrentHealth.ToString("000");
    }
}
