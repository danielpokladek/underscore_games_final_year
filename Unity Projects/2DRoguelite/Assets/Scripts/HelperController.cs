using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperController : MonoBehaviour
{
    public ParticleSystem pSystem;
    public PlayerStats pStats;

    private void Update()
    {
        if (pStats.currentHealth < (pStats.characterHealth.GetValue() / 3))
            pSystem.startColor = Color.red;

        if (pStats.currentHealth < (pStats.characterHealth.GetValue() / 2))
            pSystem.startColor = Color.yellow;

        if (pStats.currentHealth > (pStats.characterHealth.GetValue() / 2))
            pSystem.startColor = Color.green;

        if (pStats.currentHealth == pStats.characterHealth.GetValue())
            pSystem.startColor = Color.blue;
    }
}
