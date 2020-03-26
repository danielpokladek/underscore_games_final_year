using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    public PlayerHearts playerHearts;
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

    [HideInInspector] public bool lowHealth = false;
    [HideInInspector] public bool hasElixir = false;

    private PlayerController playerController;
    private CanvasGroup heartCanvasGroup;

    private event EventHandler OnStatChange;

    override protected void Start()
    {
        base.Start();

        playerController = GetComponent<PlayerController>();    
        LevelManager.instance.LoadPlayerStats();

        PlayerHeartSystem heartSystem = new PlayerHeartSystem((int)characterHealth.GetValue() / 10);
        playerHearts.SetHeartSystem(heartSystem);

        heartCanvasGroup = playerHearts.GetComponent<CanvasGroup>();

        OnStatChange += PlayerStats_OnStatChange;

        OnStatChange(this, EventArgs.Empty);
    }

    public override void HealCharacter(float healAmount)
    {

        base.HealCharacter(healAmount);

        if (currentHealth > ((characterHealth.GetValue() / 100) * 40))
            lowHealth = false;

        UIManager.current.updateUICallback();
        playerHearts.heartSystem.Heal(healAmount / 10);

        OnStatChange(this, EventArgs.Empty);
    }

    override public void TakeDamage(float damageAmount)
    {
        if (!playerController.playerAlive)
            return;

        if (!canTakeDamage)
            return;

        if (godMode)
            return;

        if (damageAmount != 0)
            gameUI.DamageIndicator(transform.position, damageAmount);

        // --- --- ---
        base.TakeDamage(damageAmount);

        if (currentHealth < ((characterHealth.GetValue() / 100) * 40))
        {
            lowHealth = true;
            StartCoroutine(LowHealthCoroutine());
        }

        if(playerController.onTakeDamageCallback != null)
            playerController.onTakeDamageCallback.Invoke();

        UIManager.current.updateUICallback.Invoke();
        CameraShaker.Instance.ShakeOnce(2f, 2f, .01f, .1f);

        playerHearts.heartSystem.Damage(damageAmount / 10);

        OnStatChange(this, EventArgs.Empty);

        if (playerController.playerAlive)
            StartCoroutine(Damaged());
    }

    override protected void CharacterDeath()
    {
        if (hasElixir)
        {
            hasElixir = false;
            currentHealth = characterHealth.GetValue();

            return;
        }

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
        UIManager.current.updateUICallback();
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
        while (lowHealth)
        {
            heartCanvasGroup.alpha = 1.0f;

            playerController.playerSprite.color = Color.red;

            yield return new WaitForSeconds(.55f);

            playerController.playerSprite.color = Color.white;

            yield return new WaitForSeconds(.55f);
        }

        OnStatChange(this, EventArgs.Empty);

        yield break;
    }

    bool animate = false;
    Coroutine routine = null;

    private void PlayerStats_OnStatChange(object sender, System.EventArgs e)
    {
        if (lowHealth)
            return;

        animate = false;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(HeartsFade());
    }

    private IEnumerator HeartsFade()
    {
        if (lowHealth)
            yield break;

        float alpha = heartCanvasGroup.alpha;
        animate = true;

        while (animate && !lowHealth)
        {
            for (float t = 0.01f; t < 1.0f;)
            {
                t += Time.deltaTime;
                t = Mathf.Min(t, 1.0f);

                heartCanvasGroup.alpha = Mathf.Lerp(alpha, 1, Mathf.Min(1, t / 1.0f));

                yield return null;
            }

            animate = false;
        }

        yield return new WaitForSeconds(2.0f);

        alpha = heartCanvasGroup.alpha;
        animate = true;

        while (animate && !lowHealth)
        {
            for (float t = 0.01f; t < 1.5f;)
            {
                t += Time.deltaTime;
                t = Mathf.Min(t, 1.5f);

                heartCanvasGroup.alpha = Mathf.Lerp(alpha, 0, Mathf.Min(1, t / 1.5f));

                yield return null;
            }

            animate = false;
        }

        animate = false;
        yield break;
    }
}