using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBossStageOneController : EnemyController
{
    [SerializeField] private int projectileAmount;
    [SerializeField] private float startAngle = 0.0f, endAngle = 360.0f;
    [SerializeField] private GameObject normalProjectile;
    [SerializeField] private GameObject explosiveProjectile;
    [SerializeField] private GameObject[] enemySpawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject stageTwoPrefab;

    // --- --- ---
    private Vector2 bulletMoveDirection;

    public void StartAttack(float _attackLength)
    {
        Debug.Log("Start attack");
        StartCoroutine(AttackCoroutine(_attackLength));
    }

    public void SetProjectileAmount(int _projectileAmount)
    {
        projectileAmount = _projectileAmount;
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
        float angleStep = (endAngle - startAngle) / projectileAmount;
        float angle = startAngle;

        for (int i = 0; i < projectileAmount; i++)
        {
            float bulDriX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180.0f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180.0f);

            Vector3 bulMoveVector = new Vector3(bulDriX, bulDirY, 0.0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = Instantiate(normalProjectile,
                attackPoint.position, Quaternion.identity);

                bul.GetComponent<Rigidbody2D>().AddForce(bulDir * 5, ForceMode2D.Impulse);
                bul.GetComponent<Projectile>().SetDamage(enemyStats.characterAttackDamage.GetValue());

                angle += angleStep;
        }
    }

    public void MoveToPlayer()
    {
        transform.position = enemyMovement.playerTrans.position;
    }

    public void ExplosiveProjectile()
    {
        GameObject proj = Instantiate(explosiveProjectile, transform.position, Quaternion.identity);

        proj.GetComponent<BossBullet>().Bullet(enemyMovement.playerTrans, 5.0f, enemyStats.characterAttackDamage.GetValue());
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
        Instantiate(stageTwoPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}