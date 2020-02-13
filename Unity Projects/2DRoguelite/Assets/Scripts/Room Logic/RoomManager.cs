using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Tooltip("Leave this option check if you want the enemies to be spawned.")]
    [SerializeField] protected bool spawnEnemies = true;
    [Tooltip("These are the points where the enemies will spawn in the room, " +
        "make sure that the object has a EnemySpawner script attached to it.")]
    [SerializeField] protected GameObject[] enemySpawnPoints;
    [Tooltip("These are the points where the shops will spawn in the room, " +
        "the script will select one of those points when creating the room.")]
    [SerializeField] protected GameObject[] shopSpawnPoints;
    [Tooltip("These are the points where the portal will spawn in the room, " +
        "the script will select one of those points when creating the room.")]
    [SerializeField] protected GameObject[] portalSpawnPoints;

    // ---------------------------------
    protected LevelManager levelManager;

    // ------------------------------
    protected GameObject bossPortalRef;
    protected GameObject bossIconRef;
    protected GameObject shopIconRef;

    // ---------------------------
    protected bool roomDiscovered;
    [HideInInspector] public int roomType { get; set; }

    
    protected bool isPortalRoom;
    protected bool isSpawnRoom;
    protected bool isShopRoom;

    protected int enemiesSpawned;

    private void Start()
    {
        levelManager = LevelManager.instance;
        levelManager.onDayStateChangeCallback += UpdateRoomState;
    }

    public void SetRoomType(int _roomType)
    {
        roomType = _roomType;
        SpawnRoomObjects();
    }

    protected void SpawnRoomObjects()
    {
        if (roomType == 2)
            SpawnBossPortal();

        if (roomType == 3)
            SpawnItemShop();

        if (roomType == 1)
            SpawnPlayer();
    }

    virtual protected void SpawnBossPortal()
    {
        if (roomType != 0)
            return;

        bossIconRef = Instantiate(LevelManager.instance.minimapBoss, transform.position, Quaternion.identity);
    }

    virtual protected void SpawnItemShop()
    {
        if (roomType != 0)
            return;
        
        shopIconRef = Instantiate(LevelManager.instance.minimapShop, transform.position, Quaternion.identity);
    }

    virtual protected void SpawnPlayer()
    {
        GameManager.current.playerRef = 
            Instantiate(GameManager.current.playerPrefab, transform.position, Quaternion.identity);
    }

    virtual protected void UpdateRoomState() { }

    protected void DisplayBoss(bool condition)
    {
        if (bossIconRef != null)
        {
            bossIconRef.SetActive(condition);
        }
    }

    protected void SpawnEnemies()
    {
        if (!spawnEnemies)
            return;

        foreach (GameObject spawnPoint in enemySpawnPoints)
        {
            GameObject enemy = spawnPoint.GetComponent<EnemySpawnPoint>().SpawnEnemy();
            enemy.GetComponent<EnemyController>().onEnemyDeathCallback += OnEnemyDeath;
            enemiesSpawned += 1;
        }
    }

    protected virtual void OnEnemyDeath()
    {

    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (isSpawnRoom)
            return;
    }
}
