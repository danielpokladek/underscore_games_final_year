using UnityEngine;

public class RoomSelector : MonoBehaviour
{
    [System.Serializable]
    public class RoomsList
    {
        public GameObject[] roomUp;
        public GameObject[] roomDown;
        public GameObject[] roomRight;
        public GameObject[] roomLeft;
        public GameObject[] roomUpDown;
        public GameObject[] roomRightLeft;
        public GameObject[] roomUpRight;
        public GameObject[] roomUpLeft;
        public GameObject[] roomRightDown;
        public GameObject[] roomDownLeft;
        public GameObject[] roomUpDownLeft;
        public GameObject[] roomUpRightLeft;
        public GameObject[] roomUpRightDown;
        public GameObject[] roomRightDownLeft;
        public GameObject[] roomUpDownRightLeft;
    }

    public Transform roomParent;

    [Tooltip("This is where the room prefabs are assigned for the level generator to use." +
        "This should only be adjusted on the empty prefab for the rooms," +
        "as this will ensure the changes are saved for all rooms which use this prefab.")]
    public RoomsList roomList;

    public bool up, down, left, right;
    [HideInInspector] public int roomType;

    private GameObject tempRoom;
    private int doorDest;                    // Integer used to decide what type of room this is,
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
        switch (doorDest)
        {
            case 1:
                // UP ONLY
                tempRoom = Instantiate(
                    roomList.roomUp[Random.Range(0, roomList.roomUp.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 2:
                // DOWN ONLY
                tempRoom = Instantiate(
                    roomList.roomDown[Random.Range(0, roomList.roomDown.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 4:
                // LEFT ONLY
                tempRoom = Instantiate(
                    roomList.roomLeft[Random.Range(0, roomList.roomLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 8:
                // RIGHT ONLY
                tempRoom = Instantiate(
                    roomList.roomRight[Random.Range(0, roomList.roomRight.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 3:
                // UP - DOWN
                tempRoom = Instantiate(
                    roomList.roomUpDown[Random.Range(0, roomList.roomUpDown.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 12:
                // RIGHT - LEFT
                tempRoom = Instantiate(
                    roomList.roomRightLeft[Random.Range(0, roomList.roomRightLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 9:
                // UP - RIGHT
                tempRoom = Instantiate(
                    roomList.roomUpRight[Random.Range(0, roomList.roomUpRight.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 5:
                // UP - LEFT
                tempRoom = Instantiate(
                    roomList.roomUpLeft[Random.Range(0, roomList.roomUpLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 10:
                // RIGHT - DOWN
                tempRoom = Instantiate(
                    roomList.roomRightDown[Random.Range(0, roomList.roomRightDown.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 6:
                // DOWN - LEFT
                tempRoom = Instantiate(
                    roomList.roomDownLeft[Random.Range(0, roomList.roomDownLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 7:
                // UP - DOWN - LEFT
                tempRoom = Instantiate(
                    roomList.roomUpDownLeft[Random.Range(0, roomList.roomUpDownLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 13:
                // UP - RIGHT - LEFT
                tempRoom = Instantiate(
                    roomList.roomUpRightLeft[Random.Range(0, roomList.roomUpRightLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 11:
                // UP - RIGHT - DOWN
                tempRoom = Instantiate(
                    roomList.roomUpRightDown[Random.Range(0, roomList.roomUpRightDown.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 14:
                // RIGHT - DOWN - LEFT
                tempRoom = Instantiate(
                    roomList.roomRightDownLeft[Random.Range(0, roomList.roomRightDownLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 15:
                // ALL FOUR
                tempRoom = Instantiate(
                    roomList.roomUpDownRightLeft[Random.Range(0, roomList.roomUpDownRightLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
        }
        
        if (tempRoom != null)
            tempRoom.transform.SetParent(gameObject.transform);
        
        if (roomType == 1)
            tempRoom.GetComponent<RoomManager>().SpawnPlayer();

        if (roomType == 2)
            tempRoom.GetComponent<RoomManager>().SpawnBoss();
    }
}