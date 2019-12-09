using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat characterHealth;
    public float currentHealth { get; private set; }

    public Stat characterSpeed;
    public Stat characterAttackDamage;
    public Stat characterAttackDelay;

    // ---
    protected PlayerController playerController;
    protected GameUIManager gameuiManager;

    private void Awake()
    {
        currentHealth = characterHealth.GetValue();
    }

    virtual protected void Start()
    {
        playerController     = GetComponent<PlayerController>();
        gameuiManager        = GameUIManager.currentInstance;
    }

    #region External Calls
    
    public void HealCharacter(float healAmount)
    {
        if ((currentHealth + healAmount) > characterHealth.GetValue())
        {
            currentHealth = characterHealth.GetValue();
            return;
        }

        currentHealth += healAmount;
        playerController.onGUIUpdateCallback.Invoke();
        gameuiManager.HealIndicator(transform.position, healAmount);
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        gameuiManager.DamageIndicator(transform.position, damageAmount);
        playerController.onGUIUpdateCallback.Invoke();

        if (currentHealth <= 0)
        {
            Debug.Log("Character is dead.");
            CharacterDeath();
        }
    }
    #endregion

    virtual protected void CharacterDeath() { }
}
