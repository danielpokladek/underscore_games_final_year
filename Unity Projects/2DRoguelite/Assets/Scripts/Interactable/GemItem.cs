using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemItem : InteractableItem
{
    [SerializeField] private int gemWorth;

    override public void Interact(PlayerController playerController)
    {
        //playerController.AddCurrency(gemWorth);
        GameManager.current.PlayerCurrency += gemWorth;
        Destroy(gameObject);
    }
}
