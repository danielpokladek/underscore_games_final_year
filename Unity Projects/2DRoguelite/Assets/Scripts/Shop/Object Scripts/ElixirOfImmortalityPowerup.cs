using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElixirOfImmortalityPowerup : PowerupController
{
    void Start()
    {
        playerController.playerStats.hasElixir = true;
    }
}
