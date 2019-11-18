using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private float amount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            PlayerInteract(other.gameObject);
    }

    virtual protected void PlayerInteract(GameObject player)
    {
        // This is what will happen when the player intreacts with the object.
        player.GetComponent<PlayerController>().HealPlayer(amount);
        GameUIManager.currentInstance.HealIndicator(player.transform.position, amount);
        Destroy(gameObject);
    }
}
