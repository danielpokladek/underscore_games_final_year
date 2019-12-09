using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    override protected void Start()
    {
        base.Start();

        playerController.onItemInteractCallback += OnItemInteract;
    }

    private void OnItemInteract()
    {
        
    }

    override protected void CharacterDeath()
    {
        gameObject.SetActive(false);
    }
}
