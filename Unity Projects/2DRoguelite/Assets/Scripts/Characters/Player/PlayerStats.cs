using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public Stat abilityOneCooldown;
    [Tooltip("Cooldown for the second ability of player.")]
    public Stat abilityTwoCooldown;
    [Tooltip("Cooldown for the third ability of player.")]
    public Stat abilityThreeCooldown;
    public Stat damageCooldown;
    public bool godMode;
    public bool canTakeDamage = true;
    [Tooltip("Cooldown for the first ability of player, in all cases this is the dash.")]

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
        if (!canTakeDamage)
            return;

        if (godMode)
        {
            gameUI.DamageIndicator(transform.position, damageAmount);
            return;
        }

        // --- --- ---
        base.TakeDamage(damageAmount);
        
        StartCoroutine(Damaged());

        if (playerController.onUIUpdateCallback != null)
            playerController.onUIUpdateCallback.Invoke();
        
        CameraShaker.Instance.ShakeOnce(2f, 2f, .01f, .1f);
    }

    private IEnumerator Damaged()
    {
        playerController.playerSprite.color = Color.red;
        canTakeDamage = false;

        yield return new WaitForSeconds(damageCooldown.GetValue());

        playerController.playerSprite.color = Color.white;
        canTakeDamage = true;
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
