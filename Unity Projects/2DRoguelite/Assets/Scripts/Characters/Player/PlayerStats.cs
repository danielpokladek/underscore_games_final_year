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
    public AudioClip deathMusic;

    private PlayerController playerController;
    [SerializeField] private bool lowHealth = false;

    override protected void Start()
    {
        base.Start();

        StartCoroutine(LowHealthCoroutine());

        playerController = GetComponent<PlayerController>();
        playerController.onItemInteractCallback += OnItemInteract;
         
        LevelManager.instance.LoadPlayerStats();
    }

    public override void HealCharacter(float healAmount)
    {
        base.HealCharacter(healAmount);

        UIManager.current.updateUICallback();
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

        if (currentHealth < ((characterHealth.GetValue() / 100) * 40))
        {
            lowHealth = true;
            StartCoroutine(LowHealthCoroutine());
        }
        else
        {
            lowHealth = false;
        }

        UIManager.current.updateUICallback.Invoke();
        CameraShaker.Instance.ShakeOnce(2f, 2f, .01f, .1f);
        
        StartCoroutine(Damaged());
    }

    private void OnItemInteract()
    {
        playerController.onUIUpdateCallback.Invoke();
    }

    override protected void CharacterDeath()
    {
        LevelManager.instance.playerDead = true;
        UIManager.current.PlayerDead();

        AudioManager.current.PlayMusic(deathMusic);
        AudioManager.current.PlaySFX(deathSound);

        //gameObject.SetActive(false);
        playerController.playerSprite.enabled = false;
        playerController.playerAlive = false;
    }

    public void SetHealth(float value)
    {
        currentHealth = value;
        playerController.onUIUpdateCallback.Invoke();
    }

    private IEnumerator Damaged()
    {
        playerController.playerSprite.color = Color.red;
        canTakeDamage = false;

        yield return new WaitForSeconds(damageCooldown.GetValue());

        playerController.playerSprite.color = Color.white;
        canTakeDamage = true;
    }

    private IEnumerator LowHealthCoroutine()
    {
        while (lowHealth == true)
        {
            playerController.playerSprite.color = Color.red;

            yield return new WaitForSeconds(.55f);

            playerController.playerSprite.color = Color.white;

            yield return new WaitForSeconds(.55f);
        }
    }
}
