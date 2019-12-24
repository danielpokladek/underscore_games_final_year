using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject[] enemiesList;

    public void SpawnEnemy()
    {
        int rand = Random.Range(0, enemiesList.Length);

        Instantiate(enemiesList[rand], transform.position, Quaternion.identity);
    }
}
