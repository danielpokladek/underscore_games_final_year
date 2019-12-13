using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private PlayerController playerController;
    
    override protected void Start()
    {
        base.Start();

        playerController = GetComponent<PlayerController>();
        playerController.onItemInteractCallback += OnItemInteract;
    }

    override public void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        
        playerController.onGUIUpdateCallback.Invoke();
    }

    private void OnItemInteract()
    {
        playerController.onGUIUpdateCallback.Invoke();
    }

    override protected void CharacterDeath()
    {
        gameObject.SetActive(false);
    }
}
