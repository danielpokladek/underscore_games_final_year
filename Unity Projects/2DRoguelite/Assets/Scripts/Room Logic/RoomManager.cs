using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemySpawners;

    // -------------------------------
    private LevelManager levelManager;
    private GameObject bossIconGO;
    private bool enemiesSpawned;

    // -------------------
    private bool bossRoom;
    private bool spawnRoom;

    // ------------------------------
    private string m_currentDayState;

    private void Start()
    {
        levelManager = LevelManager.instance;
        levelManager.onDayStateChangeCallback += UpdateRoomState;
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

    public void UpdateRoomState()
    {
        // At the moment nothing happens in daytime; monsters only respawn at night.
        //  At night, the boss will also spawn.

        switch (levelManager.currentState)
        {
            case LevelManager.DayState.Day:
                if (bossRoom)
                    DisplayBoss(false);

                m_currentDayState = LevelManager.instance.GetCurrentState;
                break;

            case LevelManager.DayState.Night:
                if (bossRoom)
                    DisplayBoss(true);

                m_currentDayState = LevelManager.instance.GetCurrentState;
                break;

            case LevelManager.DayState.Midnight:
                // Boss is still visible at midnight.

                m_currentDayState = LevelManager.instance.GetCurrentState;
                break;
        }
    }

    private void DisplayBoss(bool condition)
    {
        // Check if room is a boss room, and update the room.
        //  At the moment, only boss icon will appear,
        //  more functionality will be added later.
        if (bossRoom)
            bossIconGO.SetActive(condition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (bossRoom && m_currentDayState == "Night" || m_currentDayState == "Midnight")
                LevelManager.instance.LoadBossBattle();

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
