using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject testAiObject;

    public void SpawnEnemy()
    {
        Instantiate(testAiObject, transform.position, Quaternion.identity);
    }
}
