using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemySpawners;

    private bool enemiesSpawned = false;

    public void SpawnPlayer()
    {
        Instantiate(GameManager.instance.playerPrefab, transform.position, Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || enemiesSpawned) return;
        
        foreach (GameObject spawner in enemySpawners)
        {
            EnemySpawnPoint spawnPoint = spawner.GetComponent<EnemySpawnPoint>();
            spawnPoint.SpawnEnemy();
        }

        enemiesSpawned = true;
    }
}
