using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    void Start()
    {
        playerController.onItemInteractCallback += OnItemInteract;
    }

    private void OnItemInteract()
    {
        
    }
}
