using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPlayerModifier : InteractableItem
{
    [Header("Skill Modifiers")]
    [Tooltip("This value adds maximum health to the player. When players interact with this item, " +
        "the number specified here will be automatically added to their max health. To remove health, " +
        "add a '-' before the number, this will remove max health from the player.")]
    [SerializeField] private float maximumHealth;
    [Tooltip("This value adds player's current health. When players interact with this item, " +
        "the number specified here will be automatically added to their current health. To deduct health, " +
        "add a '-' before the number, this will deduct from player's current health.")]
    [SerializeField] private float currentHealth;
    [Tooltip("This value adds to player's current movement speed. When players interact with this item, " +
        "the number specified here will be automatically added to their movement speed. To deduct speed, " +
        "add a '-' before the number, this will deduct from player's movement speed.")]
    [SerializeField] private float movementSpeed;
    [Tooltip("This value adds to player's attack damage. When players interact with this item," +
        "the number specified here will be automatically added to their attack damage amount. To deduct damage," +
        "add a '-' before the number, this will deduct from player's current attack damage amount.")]
    [SerializeField] private float attackDamage;
    [Tooltip("This value adds to player's current attack speed ('delay'). When players interact with this item, " +
        "the number specified here will be automatically added to their attack speed. To deduct attack speed, " +
        "add a '-' before the number, this will deduct from player's attack speed.")]
    [SerializeField] private float attackSpeed;
    [Tooltip("This value sets player's current collider size. When players interact wiwth this item, " +
        "the number specified here will be automatically set as player's collider. Keep in mind that, " +
        "that making the collider too small might have negative impact on the game.")]
    [SerializeField] private float playerCollider;
    [Tooltip("Check this box if this item adds a powerup to the player, and below add the item to add.")]
    [SerializeField] private bool addsItem;
    [Tooltip("This item will be added to player's powerup list when players interact with the item.")]
    [SerializeField] private GameObject itemToAdd;

    public override void Interact(PlayerController playerController)
    {
        if (!CheckGems())
            return;

        if (maximumHealth != 0)
            playerController.playerStats.characterHealth.AddModifier(maximumHealth);

        if (currentHealth != 0)
            playerController.playerStats.HealCharacter(currentHealth);

        if (movementSpeed != 0)
            playerController.playerStats.characterSpeed.AddModifier(movementSpeed);

        if (attackDamage != 0)
            playerController.playerStats.characterAttackDamage.AddModifier(attackDamage);

        if (attackSpeed != 0)
            playerController.playerStats.characterAttackDelay.AddModifier(attackSpeed);

        if (playerCollider != 0)
            playerController.GetComponent<CircleCollider2D>().radius = playerCollider;

        if (addsItem)
        {
            GameObject temp = Instantiate(itemToAdd, playerController.transform.position, Quaternion.identity);

            temp.GetComponent<PowerupController>().Item(playerController);
            temp.transform.SetParent(playerController.powerUpContainer);

            playerController.AddEnergyBall(itemToAdd);
        }

        Destroy(gameObject);
    }
}
