using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject[] enemiesList;

    private GameObject tempEnemy;

    public GameObject SpawnEnemy()
    {
        int rand = Random.Range(0, enemiesList.Length);

        tempEnemy = Instantiate(enemiesList[rand], transform.position, Quaternion.identity);

        return tempEnemy;
    }
}
