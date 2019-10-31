using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHoamingBullet : BossBullet
{
    private void Update()
    {
        SeekTarget();
    }

    private void SeekTarget()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, bulletTarget.position, (bulletSpeed * Time.deltaTime));
        
        // Rotate the bullet towards the target.
        direction = bulletTarget.position - transform.position;
        angle     = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;
        
        // Apply the rotation to the rigidbody.
        bulletRb.rotation = angle;
    }
}
