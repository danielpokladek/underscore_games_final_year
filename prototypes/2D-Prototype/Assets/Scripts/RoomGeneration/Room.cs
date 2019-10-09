using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private RoomTemplates templates;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        AddRoomToList();
    }

    private void AddRoomToList()
    {
        templates.levelRooms.Add(this.gameObject);
    }
}
