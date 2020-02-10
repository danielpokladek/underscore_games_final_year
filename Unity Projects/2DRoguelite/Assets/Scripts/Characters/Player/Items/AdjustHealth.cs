using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHealth : InteractableItem
{
    public enum Type { Heal, Damage, MaxHealth }
    public Type ItemType = Type.Heal;
    public float amount;

    override public void Interact(PlayerController playerController)
    {
        switch (ItemType)
        {
            case Type.Heal:
                playerController.playerStats.HealCharacter(amount);
                break;

            case Type.Damage:
                playerController.playerStats.TakeDamage(amount);
                break;

            case Type.MaxHealth:
                playerController.playerStats.characterHealth.AddModifier(amount);
                break;
        }

        Destroy(gameObject);
    }
}
