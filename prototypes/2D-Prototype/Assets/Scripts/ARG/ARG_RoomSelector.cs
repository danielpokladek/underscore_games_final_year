using System.Collections;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

public class ARG_RoomSelector : MonoBehaviour
{
    [System.Serializable]
    public class RoomsList
    {
        public GameObject[] m_roomUp;
        public GameObject[] m_roomDown;
        public GameObject[] m_roomRight;
        public GameObject[] m_roomLeft;
        public GameObject[] m_roomUpDown;
        public GameObject[] m_roomRightLeft;
        public GameObject[] m_roomUpRight;
        public GameObject[] m_roomUpLeft;
        public GameObject[] m_roomRightDown;
        public GameObject[] m_roomDownLeft;
        public GameObject[] m_roomUpDownLeft;
        public GameObject[] m_roomUpRightLeft;
        public GameObject[] m_roomUpRightDown;
        public GameObject[] m_roomRightDownLeft;
        public GameObject[] m_roomUpDownRightLeft;
    }

    public Transform m_RoomParent;

    [Tooltip("This is where the room prefabs are assigned for the level generator to use." +
        "This should only be adjusted on the empty prefab for the rooms," +
        "as this will ensure the changes are saved for all rooms which use this prefab.")]
    public RoomsList m_RoomsList;

    public bool up, down, left, right;
    [HideInInspector] public int type;

    private GameObject tempRoom = null;
    private int roomType;                    // Integer used to decide what type of room this is,
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
        // Check the bools, and add up the integers together.
        if (up)
            roomType += 1;

        if (down)
            roomType += 2;

        if (left)
            roomType += 4;

        if (right)
            roomType += 8;
    }

    private void PickRoom()
    {
        switch (roomType)
        {
            case 1:
                // UP ONLY
                tempRoom = Instantiate(
                    m_RoomsList.m_roomUp[Random.Range(0, m_RoomsList.m_roomUp.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 2:
                // DOWN ONLY
                tempRoom = Instantiate(
                    m_RoomsList.m_roomDown[Random.Range(0, m_RoomsList.m_roomDown.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 4:
                // LEFT ONLY
                tempRoom = Instantiate(
                    m_RoomsList.m_roomLeft[Random.Range(0, m_RoomsList.m_roomLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 8:
                // RIGHT ONLY
                tempRoom = Instantiate(
                    m_RoomsList.m_roomRight[Random.Range(0, m_RoomsList.m_roomRight.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 3:
                // UP - DOWN
                tempRoom = Instantiate(
                    m_RoomsList.m_roomUpDown[Random.Range(0, m_RoomsList.m_roomUpDown.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 12:
                // RIGHT - LEFT
                tempRoom = Instantiate(
                    m_RoomsList.m_roomRightLeft[Random.Range(0, m_RoomsList.m_roomRightLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 9:
                // UP - RIGHT
                tempRoom = Instantiate(
                    m_RoomsList.m_roomUpRight[Random.Range(0, m_RoomsList.m_roomUpRight.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 5:
                // UP - LEFT
                tempRoom = Instantiate(
                    m_RoomsList.m_roomUpLeft[Random.Range(0, m_RoomsList.m_roomUpLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 10:
                // RIGHT - DOWN
                tempRoom = Instantiate(
                    m_RoomsList.m_roomRightDown[Random.Range(0, m_RoomsList.m_roomRightDown.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 6:
                // DOWN - LEFT
                tempRoom = Instantiate(
                    m_RoomsList.m_roomDownLeft[Random.Range(0, m_RoomsList.m_roomDownLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 7:
                // UP - DOWN - LEFT
                tempRoom = Instantiate(
                    m_RoomsList.m_roomUpDownLeft[Random.Range(0, m_RoomsList.m_roomUpDownLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 13:
                // UP - RIGHT - LEFT
                tempRoom = Instantiate(
                    m_RoomsList.m_roomUpRightLeft[Random.Range(0, m_RoomsList.m_roomUpRightLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 11:
                // UP - RIGHT - DOWN
                tempRoom = Instantiate(
                    m_RoomsList.m_roomUpRightDown[Random.Range(0, m_RoomsList.m_roomUpRightDown.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 14:
                // RIGHT - DOWN - LEFT
                tempRoom = Instantiate(
                    m_RoomsList.m_roomRightDownLeft[Random.Range(0, m_RoomsList.m_roomRightDownLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
            
            case 15:
                // ALL FOUR
                tempRoom = Instantiate(
                    m_RoomsList.m_roomUpDownRightLeft[Random.Range(0, m_RoomsList.m_roomUpDownRightLeft.Length)],
                    transform.position, Quaternion.identity);
                break;
        }
        
        if (tempRoom != null)
            tempRoom.transform.SetParent(this.gameObject.transform);
    }
}
