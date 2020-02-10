using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAttack : InteractableItem
{
    [Header("Attack Properties:")]
    public float damageModifier;
    public float delayModifier;

    override public void Interact(PlayerController playerController)
    {
        playerController.playerStats.characterAttackDamage.AddModifier(damageModifier);
        playerController.playerStats.characterAttackDelay.AddModifier(delayModifier);
    }
}
