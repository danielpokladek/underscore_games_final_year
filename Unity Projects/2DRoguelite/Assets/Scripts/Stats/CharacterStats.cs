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

    private GameUIManager gameUI;
    
    private void Awake()
    {
        gameUI = GameUIManager.currentInstance;
        currentHealth = characterHealth.GetValue();
    }

    virtual protected void Start() { gameUI = GameUIManager.currentInstance; }

    #region External Calls
    
    virtual public void HealCharacter(float healAmount)
    {
        if ((currentHealth + healAmount) > characterHealth.GetValue())
        {
            currentHealth = characterHealth.GetValue();
            return;
        }

        currentHealth += healAmount;
        gameUI.HealIndicator(transform.position, healAmount);
    }

    virtual public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        gameUI.DamageIndicator(transform.position, damageAmount);

        if (currentHealth <= 0)
        {
            Debug.Log("Character is dead.");
            CharacterDeath();
        }
    }

    public bool IsHealed()
    {
        return currentHealth >= characterHealth.GetValue();
    }

    #endregion

    virtual protected void CharacterDeath() { }
}
