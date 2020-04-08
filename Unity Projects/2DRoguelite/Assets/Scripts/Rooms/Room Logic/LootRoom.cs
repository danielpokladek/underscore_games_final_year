using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootRoom : RoomManager
{
    public GameObject doorTilemap;

    private int enemiesKilled = 0;

    public void ChestOpened()
    {
        doorTilemap.SetActive(true);

        SpawnEnemies();
    }

    protected override void OnEnemyDeath()
    {
        enemiesKilled += 1;

        if (enemiesKilled == enemiesSpawned)
            doorTilemap.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        base.OnTriggerEnter2D(coll);

        if (coll.CompareTag("Player"))
            LevelManager.instance.PlayCaveMusic();
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
            LevelManager.instance.PlayNormalMusic();
    }
}
