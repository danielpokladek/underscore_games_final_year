using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    virtual protected void PlayerInteract(PlayerController playerController)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            PlayerInteract(other.GetComponent<PlayerController>());
    }
}
