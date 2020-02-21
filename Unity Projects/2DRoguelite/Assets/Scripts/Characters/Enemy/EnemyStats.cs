using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyStats : CharacterStats
{
    [SerializeField] private GameObject soulParticle;

    private bool  isBleeding = false;
    private float bleedingDamage;

    private EnemyController enemyController;

    override protected void Start()
    {
        base.Start();

        enemyController = GetComponent<EnemyController>();
    }

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

    override protected void CharacterDeath()
    {
        if (enemyController.onEnemyDeathCallback != null)
            enemyController.onEnemyDeathCallback.Invoke();

        if (soulParticle != null)
            if (GameManager.current.bossPortalRef != null)
                Instantiate(soulParticle, transform.position, Quaternion.identity);

        //if (Random.value <= enemyController.dropPercentage)
        //{
        //    Instantiate(enemyController.gemDrops[Random.Range(0, enemyController.gemDrops.Length -1)], transform.position, Quaternion.identity);
        //}

        Destroy(gameObject);
    }
}
