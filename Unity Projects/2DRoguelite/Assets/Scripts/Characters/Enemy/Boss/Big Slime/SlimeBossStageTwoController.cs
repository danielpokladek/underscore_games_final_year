﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossStageTwoController : EnemyController
{
    [SerializeField] private Transform aimContainer;
    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private GameObject homingProjectile;
    [SerializeField] private GameObject[] enemySpawnPoints;
    [SerializeField] private GameObject enemyPrefab;

    private float angle;

    // --- --- ---
    private Vector2 bulletMoveDirection;

    override protected void Update()
    {
        base.Update();

        aimContainer.Rotate(0, 0, 15 * Time.deltaTime);
    }

    public void StartAttack(float _attackLength)
    {
        Debug.Log("Start attack");
        StartCoroutine(AttackCoroutine(_attackLength));
    }

    private IEnumerator AttackCoroutine(float attackLength)
    {
        Debug.Log("Attack coroutine");

        InvokeRepeating("Fire", 0.0f, 2.0f);

        yield return new WaitForSeconds(attackLength);

        CancelInvoke();

        yield break;
    }

    public void Fire()
    {
        float angleStep = (0 - 360) / 18;

        for (int i = 0; i < 18; i++)
        {
            float bulDriX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180.0f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180.0f);

            Vector3 bulMoveVector = new Vector3(bulDriX, bulDirY, 0.0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            //GameObject bul = Instantiate(normalProjectile,
            //    attackPoint.position, Quaternion.identity);

            GameObject proj = ObjectPooler.instance.PoolItem("bossNormal",
                attackPoint.position, Quaternion.identity);

                proj.GetComponent<Rigidbody2D>().AddForce(bulDir * 5, ForceMode2D.Impulse);
                proj.GetComponent<Projectile>().SetDamage(enemyStats.characterAttackDamage.GetValue());

                angle += angleStep;
        }

        angle += 15;
    }

    public void MoveToPlayer()
    {
        transform.position = enemyMovement.playerTrans.position;
    }

    public void ExpandingBarrier()
    {
        float angleStep = (0 - 360) / 90;
        float angle = 0;

        for (int i = 0; i < 90; i++)
        {
            float bulDriX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180.0f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180.0f);

            Vector3 bulMoveVector = new Vector3(bulDriX, bulDirY, 0.0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            //GameObject bul = Instantiate(normalProjectile,
            //    attackPoint.position, Quaternion.identity);

            GameObject proj = ObjectPooler.instance.PoolItem("bossNormal",
                transform.position, Quaternion.identity);

                proj.GetComponent<Rigidbody2D>().AddForce(bulDir * 5, ForceMode2D.Impulse);
                proj.GetComponent<Projectile>().SetDamage(enemyStats.characterAttackDamage.GetValue());

                angle += angleStep;
        }

        GameObject homingProj = Instantiate(normalProjectile, attackPoint.position, Quaternion.identity);
            homingProj.GetComponent<Rigidbody2D>().AddForce(enemyMovement.playerVector * 5, ForceMode2D.Impulse);
            homingProj.GetComponent<Projectile>().SetDamage(enemyStats.characterAttackDamage.GetValue());
        
        Destroy(homingProj, 3.5f);

    }

    public void SpawnEnemies()
    {
        foreach (GameObject _go in enemySpawnPoints)
        {
            Instantiate(enemyPrefab, _go.transform.position, Quaternion.identity);
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}