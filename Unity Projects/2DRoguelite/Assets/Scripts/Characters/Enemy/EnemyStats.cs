using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private bool  isBleeding = false;
    private float bleedingDamage;

    private EnemyController enemyController;

    public void DamageOverTime(float effectLength, float damageAmount)
    {
        if (isBleeding)
            return;

        bleedingDamage = damageAmount;

        InvokeRepeating("BleedingDamage", 0.0f, 1.0f);
        StartCoroutine(BleedingDamageTimer(effectLength));
    }

    private void BleedingDamage()
    {
        TakeDamage(bleedingDamage);
    }

    private IEnumerator BleedingDamageTimer(float effectLength)
    {
        yield return new WaitForSeconds(effectLength);

        CancelInvoke("BleedingDamage");
        isBleeding = false;
    }
}
