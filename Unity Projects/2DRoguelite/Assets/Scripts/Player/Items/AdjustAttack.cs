using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAttack : ItemPickup
{
    [Header("Attack Properties:")]
    public float damageAmount;
    public float attackDelay;

    override protected void PlayerInteract(PlayerController playerController)
    {
        playerController.DamageAmount = damageAmount;
        playerController.AttackDelay = attackDelay;
    }
}
