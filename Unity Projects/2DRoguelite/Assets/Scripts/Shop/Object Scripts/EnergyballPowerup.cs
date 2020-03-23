using UnityEngine;

public class EnergyballPowerup : PowerupController
{
    void Update()
    {
        if (playerController)
            transform.RotateAround(playerController.transform.position,
                new Vector3(0, 0, 1), Time.deltaTime * 90);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            other.GetComponent<EnemyController>().enemyStats.TakeDamage(
                playerController.playerStats.characterAttackDamage.GetValue());
    }
}
