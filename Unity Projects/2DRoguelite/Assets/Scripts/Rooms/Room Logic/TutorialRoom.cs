using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom : RoomManager
{
    public bool opensRoom;
    public GameObject roomDoor;

    public void DialFinished()
    {
        if (opensRoom)
            roomDoor.SetActive(false);
    }
}
