﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Throwable : Projectile
{
    [SerializeField] private float explosionRadius;

    private void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0, 0, 1), 120 * Time.deltaTime);
    }

    override protected void DestroyProjectile()
    {
        GameObject fx = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(fx, 0.6f);

        Collider2D[] enemiesInRange = null;
        enemiesInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.NameToLayer("Enemy"));

        foreach (Collider2D coll in enemiesInRange)
        {
            if (coll.CompareTag("Enemy"))
                coll.GetComponent<EnemyController>().enemyStats.TakeDamage(projectileDamage);
        }

        CameraShaker.Instance.ShakeOnce(15.0f, 8.0f, .5f, .5f);

        projectileRB.velocity = Vector3.zero;
        ObjectPooler.instance.AddItem(projectileTag, gameObject);
        gameObject.SetActive(false);
    }

    protected void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
            DestroyProjectile();

        if (coll.gameObject.layer == LayerMask.NameToLayer("Environemnt"))
            DestroyProjectile();
    }
}