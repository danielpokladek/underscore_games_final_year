using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemItem : ItemPickup
{
    [SerializeField] private int gemWorth;

    override protected void PlayerInteract(PlayerController playerController)
    {
        //playerController.AddCurrency(gemWorth);
        GameManager.current.PlayerCurrency += gemWorth;
        Destroy(gameObject);
    }
}
