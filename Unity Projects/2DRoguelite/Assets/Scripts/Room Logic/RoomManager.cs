using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemySpawners;
    [SerializeField] private GameObject[] shopSpawner;

    // -------------------------------
    private LevelManager levelManager;
    private GameObject bossIconGO;
    private GameObject shopIcon;
    private bool enemiesSpawned;

    // -------------------
    private bool bossRoom;
    private bool spawnRoom;
    private bool shopRoom;

    // ------------------------------
    private string m_currentDayState;
    private GameObject m_bossPortal;

    private void Start()
    {
        levelManager = LevelManager.instance;
        levelManager.onDayStateChangeCallback += UpdateRoomState;

        if (bossRoom)
        {
            m_bossPortal = Instantiate(LevelManager.instance.bossPortal, transform.position, Quaternion.identity);
            GameManager.current.bossPortal = m_bossPortal;
            m_bossPortal.SetActive(false);
        }
    }

    public void SetSpawnRoom()
    {
        spawnRoom = true;
        Instantiate(GameManager.current.playerPrefab, transform.position, Quaternion.identity);
    }

    public void SetBossRoom(GameObject bossIcon)
    {
        bossIconGO = Instantiate(bossIcon, transform.position, Quaternion.identity);
        bossIconGO.SetActive(false);
        bossRoom = true;
    }

    public void SetShopRoom(GameObject _shopIcon)
    {
        shopRoom = true;

        int rand = Random.Range(0, shopSpawner.Length);

        GameObject shop = Instantiate(LevelManager.instance.shopPrefab,
            shopSpawner[rand].transform.position, Quaternion.identity);
        shopIcon = Instantiate(_shopIcon, transform.position, Quaternion.identity);
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
        {
            bossIconGO.SetActive(condition);
            m_bossPortal.SetActive(condition);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawnRoom)
                return;

            // It is daytime, and enemies have been spawned.
            if (levelManager.GetCurrentState == "Day" && enemiesSpawned)
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
