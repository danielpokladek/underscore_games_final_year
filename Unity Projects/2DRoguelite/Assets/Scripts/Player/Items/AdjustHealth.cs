using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHealth : ItemPickup
{
    public enum Type { Heal, Damage, MaxHealth }
    public Type ItemType = Type.Heal;
    public float amount;

    override protected void PlayerInteract(PlayerController playerController)
    {
        switch (ItemType)
        {
            case Type.Heal:
                playerController.HealPlayer(amount);
                break;

            case Type.Damage:
                playerController.TakeDamage(amount);
                break;

            case Type.MaxHealth:
                playerController.SetMaxHealth(amount);
                break;
        }

        Destroy(gameObject);
    }
}
