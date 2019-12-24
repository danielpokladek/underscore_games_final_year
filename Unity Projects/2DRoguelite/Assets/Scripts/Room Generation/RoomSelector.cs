using UnityEngine;

public class RoomSelector : MonoBehaviour
{
    public RoomContainer entranceRooms;
    
    [Tooltip("This is where the room prefabs are assigned for the level generator to use." +
        "This should only be adjusted on the empty prefab for the rooms," +
        "as this will ensure the changes are saved for all rooms which use this prefab.")]
    public RoomContainer roomContainer;

    [Header("Temporary")]
    public GameObject bossIcon;
    public GameObject shopIcon;
    
    public int sceneToLoad;

    // -------------------------------------------------
    [HideInInspector] public Transform roomParent;
    [HideInInspector] public bool up, down, left, right;
    [HideInInspector] public int roomType;
    private GameObject  tempRoom;
    private int         doorDest;   // Integer used to decide what type of room this is,
                                    //    based on 1-2-4-8. 1 up, 2 down, 4 left, 8 right.
                                    //    This way I can add the integers together, to decide
                                    //    what type of room this will be.
                                    //        For example 1+2 (up + down) = 3.
                                    //        No other combination will give me this result.
                                    //        The same applies to other combinations.
    
    private void Start()
    {
        CheckRoomType();
        PickRoom();
    }

    private void CheckRoomType()
    {
        // Check the bool status, and add up the integers together.
        if (up)
            doorDest += 1;

        if (down)
            doorDest += 2;

        if (left)
            doorDest += 4;

        if (right)
            doorDest += 8;
    }

    private void PickRoom()
    {
        if (roomType == 1)
        {
            switch (doorDest)
            {
                case 1:
                    // UP ONLY
                    tempRoom = Instantiate(
                        entranceRooms.roomUp[Random.Range(0, entranceRooms.roomUp.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 2:
                    // DOWN ONLY
                    tempRoom = Instantiate(
                        entranceRooms.roomDown[Random.Range(0, entranceRooms.roomDown.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 4:
                    // LEFT ONLY
                    tempRoom = Instantiate(
                        entranceRooms.roomLeft[Random.Range(0, entranceRooms.roomLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 8:
                    // RIGHT ONLY
                    tempRoom = Instantiate(
                        entranceRooms.roomRight[Random.Range(0, entranceRooms.roomRight.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 3:
                    // UP - DOWN
                    tempRoom = Instantiate(
                        entranceRooms.roomUpDown[Random.Range(0, entranceRooms.roomUpDown.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 12:
                    // RIGHT - LEFT
                    tempRoom = Instantiate(
                        entranceRooms.roomRightLeft[Random.Range(0, entranceRooms.roomRightLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 9:
                    // UP - RIGHT
                    tempRoom = Instantiate(
                        entranceRooms.roomUpRight[Random.Range(0, entranceRooms.roomUpRight.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 5:
                    // UP - LEFT
                    tempRoom = Instantiate(
                        entranceRooms.roomUpLeft[Random.Range(0, entranceRooms.roomUpLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 10:
                    // RIGHT - DOWN
                    tempRoom = Instantiate(
                        entranceRooms.roomRightDown[Random.Range(0, entranceRooms.roomRightDown.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 6:
                    // DOWN - LEFT
                    tempRoom = Instantiate(
                        entranceRooms.roomDownLeft[Random.Range(0, entranceRooms.roomDownLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 7:
                    // UP - DOWN - LEFT
                    tempRoom = Instantiate(
                        entranceRooms.roomUpDownLeft[Random.Range(0, entranceRooms.roomUpDownLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 13:
                    // UP - RIGHT - LEFT
                    tempRoom = Instantiate(
                        entranceRooms.roomUpRightLeft[Random.Range(0, entranceRooms.roomUpRightLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 11:
                    // UP - RIGHT - DOWN
                    tempRoom = Instantiate(
                        entranceRooms.roomUpRightDown[Random.Range(0, entranceRooms.roomUpRightDown.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 14:
                    // RIGHT - DOWN - LEFT
                    tempRoom = Instantiate(
                        entranceRooms.roomRightDownLeft[Random.Range(0, entranceRooms.roomRightDownLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 15:
                    // ALL FOUR
                    tempRoom = Instantiate(
                        entranceRooms.roomUpDownRightLeft[Random.Range(0, entranceRooms.roomUpDownRightLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;
            }
        }

        if (roomType != 1)
        {
            switch (doorDest)
            {
                case 1:
                    // UP ONLY
                    tempRoom = Instantiate(
                        roomContainer.roomUp[Random.Range(0, roomContainer.roomUp.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 2:
                    // DOWN ONLY
                    tempRoom = Instantiate(
                        roomContainer.roomDown[Random.Range(0, roomContainer.roomDown.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 4:
                    // LEFT ONLY
                    tempRoom = Instantiate(
                        roomContainer.roomLeft[Random.Range(0, roomContainer.roomLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 8:
                    // RIGHT ONLY
                    tempRoom = Instantiate(
                        roomContainer.roomRight[Random.Range(0, roomContainer.roomRight.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 3:
                    // UP - DOWN
                    tempRoom = Instantiate(
                        roomContainer.roomUpDown[Random.Range(0, roomContainer.roomUpDown.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 12:
                    // RIGHT - LEFT
                    tempRoom = Instantiate(
                        roomContainer.roomRightLeft[Random.Range(0, roomContainer.roomRightLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 9:
                    // UP - RIGHT
                    tempRoom = Instantiate(
                        roomContainer.roomUpRight[Random.Range(0, roomContainer.roomUpRight.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 5:
                    // UP - LEFT
                    tempRoom = Instantiate(
                        roomContainer.roomUpLeft[Random.Range(0, roomContainer.roomUpLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 10:
                    // RIGHT - DOWN
                    tempRoom = Instantiate(
                        roomContainer.roomRightDown[Random.Range(0, roomContainer.roomRightDown.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 6:
                    // DOWN - LEFT
                    tempRoom = Instantiate(
                        roomContainer.roomDownLeft[Random.Range(0, roomContainer.roomDownLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 7:
                    // UP - DOWN - LEFT
                    tempRoom = Instantiate(
                        roomContainer.roomUpDownLeft[Random.Range(0, roomContainer.roomUpDownLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 13:
                    // UP - RIGHT - LEFT
                    tempRoom = Instantiate(
                        roomContainer.roomUpRightLeft[Random.Range(0, roomContainer.roomUpRightLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 11:
                    // UP - RIGHT - DOWN
                    tempRoom = Instantiate(
                        roomContainer.roomUpRightDown[Random.Range(0, roomContainer.roomUpRightDown.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 14:
                    // RIGHT - DOWN - LEFT
                    tempRoom = Instantiate(
                        roomContainer.roomRightDownLeft[Random.Range(0, roomContainer.roomRightDownLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;

                case 15:
                    // ALL FOUR
                    tempRoom = Instantiate(
                        roomContainer.roomUpDownRightLeft[Random.Range(0, roomContainer.roomUpDownRightLeft.Length)],
                        transform.position, Quaternion.identity);
                    break;
            }
        }
        
        if (tempRoom != null)
            tempRoom.transform.SetParent(gameObject.transform);
        
        if (roomType == 1)
            tempRoom.GetComponent<RoomManager>().InitRoom(false, true, false);

        if (roomType == 2)
            tempRoom.GetComponent<RoomManager>().InitRoom(true, false, false);
        
        if (roomType == 3)
            tempRoom.GetComponent<RoomManager>().InitRoom(false, false, true);
    }
}