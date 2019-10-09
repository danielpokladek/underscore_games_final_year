using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    [Header("Room Types")]
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject wallRoom;

    [Header("Generation Settings")]
    [SerializeField] private float waitTime;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject shop;

    [HideInInspector] public List<GameObject> spawnPointsInRoom;
    [HideInInspector] public List<GameObject> levelRooms;
    [HideInInspector] public bool spawned = false;

    private bool spawnedBoss    = false;
    private bool spawnedShop    = false;

    private void Update()
    {
        if (waitTime <= 0)
        {
            // Wait for timer to run out, before spawning special rooms.
            // This allows the level to fully deploy.

            if (spawnedBoss == false)
            {
                Instantiate(boss, levelRooms[levelRooms.Count - 1].transform.position, Quaternion.identity);
                spawnedBoss = true;
            }

            if (spawnedShop == false)
            {
                int randRoom = Random.Range(3, levelRooms.Count - 1);
                Instantiate(shop, levelRooms[randRoom].transform.position, Quaternion.identity);
                spawnedShop = true;
            }

            // By this point the dungeon is generated, and we can clear the spawn points to save memory.
            // Ideally there should be a function that tells the script the generation is done, and we can clear the list.
            spawned = true;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
