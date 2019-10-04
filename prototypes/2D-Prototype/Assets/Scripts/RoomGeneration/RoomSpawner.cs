using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private int openingDirection = 0;
    // 1 - requires bottom door
    // 2 - requires top door
    // 3 - requires left door
    // 4 - requires right door
    // ---
    // 10 - reserved for entry room ONLY (place in middle of room)

    private RoomTemplates templates;
    private bool spawned = false;
    private int rand;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.spawnPointsInRoom.Add(this.gameObject);

        Invoke("SpawnRoom", 0.1f);
    }

    private void SpawnRoom()
    {
        if (spawned == false)
        {
            if (openingDirection == 0)
            {
                Debug.LogError("Opening direction not set in: " + gameObject.transform.parent.transform.parent.name + ", please specify opening direction.");
            }
            else if (openingDirection == 1)
            {
                // Need to spawn a room with a BOTTOM door.
                rand = Random.Range(0, templates.bottomRooms.Length -1);
                Instantiate(templates.bottomRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 2)
            {
                // Need to spawn a room with a TOP door.
                rand = Random.Range(0, templates.topRooms.Length -1);
                Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 3)
            {
                // Need to spawn a room with a LEFT door.
                rand = Random.Range(0, templates.leftRooms.Length -1);
                Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity);
            }
            else if (openingDirection == 4)
            {
                // Need to spawn a room with a RIGHT door.
                rand = Random.Range(0, templates.rightRooms.Length -1);
                Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity);
            }

            spawned = true;
        }
        else return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().openingDirection == 5)
            {
                return;
            }
            else if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                // Spawn walls blocking off any openings to the outside world!
                Instantiate(templates.wallRoom, transform.position, Quaternion.identity);
                templates.spawnPointsInRoom.Remove(this.gameObject);
                Destroy(gameObject);
            }

            spawned = true;
        }
    }
}
