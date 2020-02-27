using UnityEngine;
using EZCameraShake;

public class PlayerProjectile : Projectile
{
    [Tooltip("Strength of the shake which will be applied to the camera upon hitting the enemy.")]
    [SerializeField] protected float cameraShakeMagnitude = 4.0f;

    [Tooltip("Rougness of the camera shake upon hitting the enemy.")]
    [SerializeField] protected float cameraShakeRoughness = 2.0f;

    protected GameObject _hitEffect;

    private void Start()
    {
        Destroy(gameObject, 3.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {     
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            EnemyHit(enemyController);
        }
        
        //if (other.gameObject.CompareTag("BossHitPoint"))
        //{
        //    other.transform.parent.GetComponent<BossController>().DamageBoss(projectileDamage);

        //    GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //    Destroy(effect, 0.4f);
        //    Destroy(gameObject);
        //}

        //if (other.gameObject.CompareTag("BossProjectile"))
        //{
        //    other.gameObject.GetComponent<BossBullet>().DamageBullet(projectileDamage);

        //    GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //    Destroy(effect, 0.4f);
        //    Destroy(gameObject);
        //}

        //if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        //{
        //    GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //    Destroy(effect, 0.4f);
        //    Destroy(gameObject);
        //}
    }

    virtual protected void EnemyHit(EnemyController enemyController)
    {
        enemyController.enemyStats.TakeDamage(projectileDamage);
        _hitEffect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //CameraShaker.Instance.ShakeOnce(cameraShakeMagnitude, cameraShakeRoughness, .1f, .5f);

        if (_hitEffect != null)
            Destroy(_hitEffect, 0.4f);

        Destroy(gameObject);
    }
}
