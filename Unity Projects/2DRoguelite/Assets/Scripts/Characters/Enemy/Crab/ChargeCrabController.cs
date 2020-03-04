using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeCrabController : EnemyMelee
{
    override protected void AttackPlayer()
    {
        StartCoroutine(HermitDash());
    }

    private IEnumerator HermitDash()
    {
        Vector2 temp = enemyMovement.playerVector;

        StartCoroutine(enemyMovement.AddForce(temp,
            Vector2.Distance(enemyMovement.playerTrans.position, transform.position) * 5));

        yield return new WaitForSeconds(.35f);

        foreach (Collider2D _player in playerInRange)
        {
            _player.GetComponent<PlayerMovement>().AddForce(
                (transform.position - _player.transform.position), 5, .5f);
                
            _player.GetComponent<PlayerController>().playerStats.TakeDamage(
                enemyStats.characterAttackDamage.GetValue());
        }

        _attackDelay = 0;
        canAttack = false;
    }
}