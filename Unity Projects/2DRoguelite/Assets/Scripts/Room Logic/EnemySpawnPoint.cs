using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private EnemyContainer enemyContainer;

    private GameObject tempEnemy;

    public GameObject SpawnEnemy()
    {
        int rand = Random.Range(0, enemyContainer.enemyPrefabs.Length);

        tempEnemy = Instantiate(enemyContainer.enemyPrefabs[rand], transform.position, Quaternion.identity);

        return tempEnemy;
    }
}
