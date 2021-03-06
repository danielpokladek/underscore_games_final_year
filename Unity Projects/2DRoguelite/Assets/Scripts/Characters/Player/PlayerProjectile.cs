﻿using UnityEngine;
using EZCameraShake;

public class PlayerProjectile : Projectile
{
    [Tooltip("Strength of the shake which will be applied to the camera upon hitting the enemy.")]
    [SerializeField] protected float cameraShakeMagnitude = 4.0f;

    [Tooltip("Rougness of the camera shake upon hitting the enemy.")]
    [SerializeField] protected float cameraShakeRoughness = 2.0f;

    protected GameObject _hitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WaterLayer"))
            return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            EnemyHit(enemyController);
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.4f);
            DestroyProjectile();
        }
    }

    virtual protected void EnemyHit(EnemyController enemyController)
    {
        enemyController.enemyStats.TakeDamage(projectileDamage);
        _hitEffect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        if (_hitEffect != null)
            Destroy(_hitEffect, 0.4f);

        DestroyProjectile();
    }
}
