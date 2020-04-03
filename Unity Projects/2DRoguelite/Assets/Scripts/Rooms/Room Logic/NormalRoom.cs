using UnityEngine;

public class NormalRoom : RoomManager
{
    public GameObject exitBlock;
    private int enemiesKilled = 0;

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

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.CompareTag("Player"))
        {
            if (isPortalRoom)
                other.GetComponent<PlayerController>().foundPortal = true;

            if ((levelManager.GetCurrentState == "Night") && spawnEnemies)
            {
                exitBlock.SetActive(true);
                levelManager.countTime = false;
            }

            if ((levelManager.GetCurrentState == "Day" || levelManager.GetCurrentState == "Boss") && roomDiscovered)
                return;

            SpawnEnemies();
            roomDiscovered = true;
        }
    }

    protected override void OnEnemyDeath()
    {
        enemiesKilled += 1;

        if (enemiesKilled == enemiesSpawned && exitBlock != null)
        {
            exitBlock.SetActive(false);
            levelManager.countTime = false;
        }
    }
}
