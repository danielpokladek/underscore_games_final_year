using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public PlayerData playerData { get; set; }

    public SaveData()
    {
        
    }
}

[System.Serializable]
public class PlayerData
{
    public float CurrentHealth      { get; set; }
    public float MaxHealth          { get; set; }
    public float MoveSpeed          { get; set; }
    public float AttackDamage       { get; set; }
    public float AttackDelay        { get; set; }
    public string ItemsList         { get; set; }

    public PlayerData(float pCurrHealth, float pMaxHealth, float pSpeed, float pAttackDmg, float pAttackDelay, string itemList = "")
    {
        this.CurrentHealth    = pCurrHealth;
        this.MaxHealth        = pMaxHealth;
        this.MoveSpeed        = pSpeed;
        this.AttackDamage     = pAttackDmg;
        this.AttackDelay      = pAttackDelay;
        this.ItemsList        = itemList;
    }
}
