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
    protected GameUIManager uiManager;

    private void Awake()
    {
        currentHealth = characterHealth.GetValue();

        playerController = GetComponent<PlayerController>();
    }

    #region External Calls
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        uiManager.DamageIndicator(transform.position, damageAmount);
        playerController.onGUIChangeCallback.Invoke();

        if (currentHealth <= 0)
        {
            Debug.Log("Character is dead.");
            CharacterDeath();
        }
    }
    #endregion

    virtual protected void CharacterDeath() { }
}
