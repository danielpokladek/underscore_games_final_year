using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeExplodeProjectile : BossBullet
{
    [SerializeField] private GameObject projectile;
    private Vector2 targetPos;

    override protected void InitProjectile()
    {
        base.InitProjectile();

        targetPos = (Vector2)bulletTarget.position;
    }

    private void Update()
    {
        if ((Vector2)transform.position == targetPos)
        {
            SpawnExtraBullets();
            DestroyBullet(0);
        }

        SeekTarget();
    }

    private void SeekTarget()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, targetPos, (bulletSpeed * Time.deltaTime));

            direction = targetPos - (Vector2)transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;

            projectileRB.rotation = angle;
    }

    private void SpawnExtraBullets()
    {
        float angleStep = (360 - 0) / 30;
        float angle = 0;

        for (int i = 0; i < 30; i++)
        {
            float bulDriX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180.0f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180.0f);

            Vector3 bulMoveVector = new Vector3(bulDriX, bulDirY, 0.0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = Instantiate(projectile,
                transform.position, Quaternion.identity);

                bul.GetComponent<Rigidbody2D>().AddForce(bulDir * 2, ForceMode2D.Impulse);
                bul.GetComponent<Projectile>().SetDamage(projectileDamage);

                angle += angleStep;
        }
    }
}
