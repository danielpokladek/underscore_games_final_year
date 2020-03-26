using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberCrabController : EnemyMelee
{
    [SerializeField] private SpriteRenderer crabSprite;

    override protected void AttackPlayer()
    {
        if (Vector2.Distance(enemyMovement.playerTrans.position, transform.position) < 10)
            StartCoroutine(HermitDash());
    }

    bool dashing = false;
    bool inRange = false;

    protected override void Update()
    {
        base.Update();

        if (Vector2.Distance(dest, transform.position) < attackRange)
            inRange = true;
        else
            inRange = false;
    }

    Vector2 dest = Vector2.zero;
    bool attacked = false;

    private IEnumerator HermitDash()
    {
        crabSprite.color = Color.red;
        enemyMovement.enableMovement = false;

        yield return new WaitForSeconds(0.4f);

        dest = enemyMovement.playerTrans.position;

        float dashStrength = Vector2.Distance(dest, transform.position);
        dashStrength = Mathf.Max(dashStrength, 30);

        crabSprite.color = Color.white;

        StartCoroutine(enemyMovement.AddForce(enemyMovement.playerVector, dashStrength));

        attacked = false;
        StartCoroutine(Attack());

        _attackDelay = 0;
        canAttack = false;
        dashing = false;
        yield return null;
    }

    private IEnumerator Attack()
    {
        while (!inRange)
        {
            yield return null;
        }

        foreach (Collider2D _player in playerInRange)
        {
            if (attacked)
                yield break;

            StartCoroutine(_player.GetComponent<PlayerMovement>().AddForce(
                (_player.transform.position - transform.position), 15, .3f));

            _player.GetComponent<PlayerController>().playerStats.TakeDamage(
                enemyStats.characterAttackDamage.GetValue());

            attacked = true;
        }
    }
}