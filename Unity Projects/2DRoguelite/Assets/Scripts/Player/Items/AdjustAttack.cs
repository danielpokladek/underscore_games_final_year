using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAttack : ItemPickup
{
    [Header("Attack Properties:")]
    public float damageModifier;
    public float delayModifier;

    override protected void PlayerInteract(PlayerController playerController)
    {
        playerController.playerStats.characterAttackDamage.AddModifier(damageModifier);
        playerController.playerStats.characterAttackDelay.AddModifier(delayModifier);
    }
}
