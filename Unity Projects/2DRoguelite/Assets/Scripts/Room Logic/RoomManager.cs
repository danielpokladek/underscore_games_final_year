using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemySpawners;
    [SerializeField] private GameObject boss;

    LevelManager levelManager;
    GameObject bossIconGO;
    bool bossRoom;
    private bool spawnRoom;
    public bool enemiesSpawned;

    private void Start()
    {
        levelManager = LevelManager.instance;
    }

    public void SpawnPlayer()
    {
        spawnRoom = true;
        Instantiate(GameManager.instance.playerPrefab, transform.position, Quaternion.identity);
    }

    public void SpawnBoss(GameObject bossIcon)
    {
        bossIconGO = Instantiate(bossIcon, transform.position, Quaternion.identity);
        bossIconGO.SetActive(false);
        bossRoom = true;
    }

    private void Update()
    {
        if (levelManager.GetCurrentState == "Night")
            ShowBoss();
    }

    private void ShowBoss()
    {
        if (bossRoom)
            bossIconGO.SetActive(true);
        else
            return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // It is daytime, and enemies have been spawned.
            if (levelManager.GetCurrentState == "Day" && enemiesSpawned)
                return;

            if (spawnRoom)
                return;
            
            foreach (GameObject spawner in enemySpawners)
            {
                EnemySpawnPoint spawnPoint = spawner.GetComponent<EnemySpawnPoint>();
                spawnPoint.SpawnEnemy();
            }

            enemiesSpawned = true;
        }    
    }
}
