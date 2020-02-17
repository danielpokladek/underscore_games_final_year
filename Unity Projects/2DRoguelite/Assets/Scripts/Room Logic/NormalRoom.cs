﻿using UnityEngine;

public class NormalRoom : RoomManager
{
    override protected void SpawnBossPortal()
    {
        base.SpawnBossPortal();

        int randPoint = Random.Range(0, portalSpawnPoints.Length);

        GameManager.current.bossPortalRef = Instantiate(LevelManager.instance.portalPrefab,
            enemySpawnPoints[randPoint].transform.position, Quaternion.identity);
    }

    override protected void SpawnItemShop()
    {
        base.SpawnItemShop();

        int randPoint = Random.Range(0, shopSpawnPoints.Length);

        bossPortalRef = Instantiate(LevelManager.instance.shopPrefab,
            shopSpawnPoints[randPoint].transform.position, Quaternion.identity);
    }

    // Doesn't work, not sure why.
    override protected void SpawnPlayer()
    {
        Debug.Log("SpawnPlayer");
        Instantiate(GameManager.current.playerPrefab, transform.position, Quaternion.identity);
    }

    override protected void UpdateRoomState()
    {
        switch (levelManager.currentState)
        {
            case LevelManager.DayState.Day:
                if (isPortalRoom)
                    DisplayBoss(false);
                break;

            case LevelManager.DayState.Night:
                if (isPortalRoom)
                    DisplayBoss(true);
                break;

            case LevelManager.DayState.Midnight:
                break;
        }
    }

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Player"))
        {
            if (isPortalRoom)
                other.GetComponent<PlayerController>().foundPortal = true;

            if (levelManager.GetCurrentState == "Day" && roomDiscovered)
                return;

            SpawnEnemies();
            roomDiscovered = true;
        }
    }
}
