using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallPowerUp : MonoBehaviour
{
    [SerializeField] private Transform rotateCenter;
    [SerializeField] private float damageAmount = 2;

    void Update()
    {
        transform.RotateAround(rotateCenter.position, new Vector3(0, 0, 1), Time.deltaTime * 90);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().enemyStats.TakeDamage(damageAmount);
        }
    }
}
