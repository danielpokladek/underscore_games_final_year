using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSpikeAttack : MonoBehaviour
{
    [Range(0.5f, 15.0f)]
    [SerializeField] private float golemAttackRange;
    [SerializeField] private float delayBeforeAttack;
    private float damageAmount;
    Collider2D[] spikesCollider;

    public LayerMask playerLayer;

    public void SpikeAttack(float _damageAmount)
    {
        damageAmount = _damageAmount;

        StartCoroutine(SpikeCoroutine());
    }

    private IEnumerator SpikeCoroutine()
    {
        yield return new WaitForSeconds(delayBeforeAttack);

        spikesCollider = Physics2D.OverlapCircleAll(transform.position, golemAttackRange, playerLayer);

        foreach (Collider2D player in spikesCollider)
        {
            player.GetComponent<PlayerController>().playerStats.TakeDamage(damageAmount);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, golemAttackRange);
    }
}
