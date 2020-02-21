using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float currentHealth { get; protected set; }
    [Tooltip("Character's max health points.")]
    public Stat characterHealth;
    [Tooltip("The speed at which the character will move.")]
    public Stat characterSpeed;
    [Tooltip("The damage which character will deal when attacking")]
    public Stat characterAttackDamage;
    [Tooltip("The delay between the attacks (in seconds).")]
    public Stat characterAttackDelay;

    protected GameUIManager gameUI;
    
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
            //Debug.Log("Character is dead.");
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
