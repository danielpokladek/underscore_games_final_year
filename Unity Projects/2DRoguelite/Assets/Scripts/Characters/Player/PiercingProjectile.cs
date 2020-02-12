using UnityEngine;
using EZCameraShake;

public class PiercingProjectile : PlayerProjectile
{
    [Tooltip("This is how many enemies the arrow will be able to hit before it is destroyed.")]
    [SerializeField] private int maxHits;

    private int arrowHits = 0;

    override protected void EnemyHit(EnemyController enemyController)
    {
        enemyController.enemyStats.TakeDamage(projectileDamage);
        //enemyController.Stun(0.7f);
        //enemyController.enemyStats.DamageOverTime(3.0f, 1.0f);

        _hitEffect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        CameraShaker.Instance.ShakeOnce(cameraShakeMagnitude, cameraShakeRoughness, .1f, .5f);

        arrowHits += 1;

        if (_hitEffect != null)
            Destroy(_hitEffect, 0.4f);

        if (arrowHits == maxHits)
            Destroy(gameObject);
    }
}
