using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyStats : CharacterStats
{
    [SerializeField] private GameObject soulParticle;

    private bool isBleeding = false;
    private bool isDead = false;
    private float bleedingDamage;

    private EnemyController enemyController;

    override protected void Start()
    {
        base.Start();

        enemyController = GetComponent<EnemyController>();
    }

    override public void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);

        StartCoroutine(EnemyDamaged());
    }

    private IEnumerator EnemyDamaged()
    {
        enemyController.enemySprite.color = Color.red;

        yield return new WaitForSeconds(0.3f);

        enemyController.enemySprite.color = Color.white;

        yield break;
    }

    public void DamageOverTime(float effectLength, float damageAmount)
    {
        if (isBleeding)
            return;

        bleedingDamage = damageAmount;

        InvokeRepeating("_DamageOverTime", 0.0f, 1.0f);
        StartCoroutine(BleedingDamageTimer(effectLength));
    }

    private void _DamageOverTime()
    {
        TakeDamage(bleedingDamage);
    }

    private IEnumerator BleedingDamageTimer(float effectLength)
    {
        yield return new WaitForSeconds(effectLength);

        CancelInvoke("_DamageOverTime");
        isBleeding = false;
    }

    override protected void CharacterDeath()
    {
        if (isDead)
            return;

        isDead = true;

        base.CharacterDeath();

        if (enemyController.onEnemyDeathCallback != null)
            enemyController.onEnemyDeathCallback.Invoke();

        if (soulParticle != null)
        {
            if (GameManager.current.bossPortalRef != null)
                Instantiate(soulParticle, transform.position, Quaternion.identity);
            else
                LevelManager.instance.AddSoul();
        }
        else
        {
            Debug.LogError("Could not find soul particle for: " + gameObject.name + "." +
                "Please make sure soul particle has been assigned.", gameObject);
        }

        Instantiate(enemyController.gemDrops[Random.Range(0, enemyController.gemDrops.Length -1)], transform.position, Quaternion.Euler(0, 0, Random.Range(-45, 45)));

        if (LevelManager.instance.onEnemyKilledCallback != null)
            LevelManager.instance.onEnemyKilledCallback.Invoke();

        GameManager.current.EnemyCount += 1;

        Destroy(gameObject);
    }
}
