﻿using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private PlayerController playerController;
    
    override protected void Start()
    {
        base.Start();

        playerController = GetComponent<PlayerController>();
        playerController.onItemInteractCallback += OnItemInteract;
         
        LevelManager.instance.LoadPlayerStats();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
            HealCharacter(10);

        if (Input.GetKeyDown(KeyCode.Minus))
            TakeDamage(10);
    }

    public override void HealCharacter(float healAmount)
    {
        base.HealCharacter(healAmount);

        playerController.onUIUpdateCallback.Invoke();
    }

    override public void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        
        playerController.onUIUpdateCallback.Invoke();
        CameraShaker.Instance.ShakeOnce(2f, 2f, .01f, .1f);
    }

    private void OnItemInteract()
    {
        playerController.onUIUpdateCallback.Invoke();
    }

    override protected void CharacterDeath()
    {
        gameObject.SetActive(false);
    }

    public void SetHealth(float value)
    {
        currentHealth = value;
        playerController.onUIUpdateCallback.Invoke();
    }
}
