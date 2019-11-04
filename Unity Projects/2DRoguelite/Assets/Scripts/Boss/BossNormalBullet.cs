using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class BossNormalBullet : BossBullet
{
    private Vector2 targetPos;

    override protected void InitBullet()
    {
        base.InitBullet();

        targetPos = (Vector2)bulletTarget.position;
    }

    private void Update()
    {
        if ((Vector2)transform.position == targetPos)
            DestroyBullet(0);
        
        SeekTarget();
    }

    private void SeekTarget()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, targetPos, (bulletSpeed * Time.deltaTime));
        
        // Rotate the bullet towards the target.
        direction = targetPos - (Vector2)transform.position;
        angle     = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
        
        // Apply the rotation to the rigidbody.
        bulletRb.rotation = angle;
    }
}
