using UnityEngine;
using EZCameraShake;

public class PiercingProjectile : PlayerProjectile
{
    [Tooltip("This is how many enemies the arrow will be able to hit before it is destroyed.")]
    [SerializeField] private int maxHits;

    private int arrowHits = 0;

    override protected void EnemyHit(EnemyController enemyController)
    {
        //base.EnemyHit(enemyController);s

        enemyController.TakeDamage(projectileDamage);

        _hitEffect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        CameraShaker.Instance.ShakeOnce(cameraShakeMagnitude, cameraShakeRoughness, .1f, .5f);

        arrowHits += 1;

        if (_hitEffect != null)
            Destroy(_hitEffect, 0.4f);

        if (arrowHits == maxHits)
            Destroy(gameObject);
    }
}
