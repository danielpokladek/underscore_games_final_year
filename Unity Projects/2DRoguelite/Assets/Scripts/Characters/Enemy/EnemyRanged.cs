using UnityEngine;

public class EnemyRanged : EnemyController
{
    [SerializeField] protected GameObject enemyProjectile;

    override protected void Update()
    {
        base.Update();
        
        if (canAttack)
            AttackPlayer();

        if (!canAttack)
        {
            _attackDelay += Time.deltaTime;

            if (_attackDelay >= enemyStats.characterAttackDelay.GetValue())
                canAttack = true;
        }
    }
}
