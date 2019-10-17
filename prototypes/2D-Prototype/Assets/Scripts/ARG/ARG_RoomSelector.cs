using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        PickRoom();
    }

    private void PickRoom()
    { 
        if (up)
        {
            if (down)
            {
                if (right)
                {
                    if (left)
                    {
                        tempRoom = Instantiate(m_RoomsList.m_roomUpDownRightLeft[
                            Random.Range(0, m_RoomsList.m_roomUpDownRightLeft.Length-1)], transform.position, Quaternion.identity);
                        //InstantiateRoom(m_RoomsList.m_roomUpDownRightLeft);
                    }
                    else
                    {
                        tempRoom = Instantiate(m_RoomsList.m_roomUpRightDown[
                            Random.Range(0, m_RoomsList.m_roomUpRightDown.Length-1)], transform.position, Quaternion.identity);
                        //InstantiateRoom(m_RoomsList.m_roomUpRightDown);
                    }
                }
                else if (left)
                {
                    tempRoom = Instantiate(m_RoomsList.m_roomUpDownLeft[
                        Random.Range(0, m_RoomsList.m_roomUpDownLeft.Length-1)], transform.position, Quaternion.identity);
                    //InstantiateRoom(m_RoomsList.m_roomUpDownLeft);
                }
                else
                {
                    tempRoom = Instantiate(m_RoomsList.m_roomUpDown[
                        Random.Range(0, m_RoomsList.m_roomUpDown.Length-1)], transform.position, Quaternion.identity);
                    //InstantiateRoom(m_RoomsList.m_roomUpDown);
                }
            }
            else
            {
                if (right)
                {
                    if (left)
                    {
                        tempRoom = Instantiate(m_RoomsList.m_roomUpRightLeft[
                            Random.Range(0, m_RoomsList.m_roomUpRightLeft.Length-1)], transform.position, Quaternion.identity);
                        //InstantiateRoom(m_RoomsList.m_roomRightDownLeft);

                    }
                    else
                    {
                        tempRoom = Instantiate(m_RoomsList.m_roomUpRight[
                            Random.Range(0, m_RoomsList.m_roomUpRight.Length-1)], transform.position, Quaternion.identity);
                        //InstantiateRoom(m_RoomsList.m_roomRightDown);
                    }
                }
                else if (left)
                {
                    tempRoom = Instantiate(m_RoomsList.m_roomDownLeft[
                            Random.Range(0, m_RoomsList.m_roomDownLeft.Length-1)], transform.position, Quaternion.identity);
                    //InstantiateRoom(m_RoomsList.m_roomDownLeft);
                }
                else
                {
                    tempRoom = Instantiate(m_RoomsList.m_roomUp[
                            Random.Range(0, m_RoomsList.m_roomUp.Length-1)], transform.position, Quaternion.identity);
                    //InstantiateRoom(m_RoomsList.m_roomDown);
                }
            }

            return;
        }

        if (down)
        {
            if (right)
            {
                if (left)
                {
                    tempRoom = Instantiate(m_RoomsList.m_roomRightDownLeft[
                            Random.Range(0, m_RoomsList.m_roomRightDownLeft.Length-1)], transform.position, Quaternion.identity);
                    //InstantiateRoom(m_RoomsList.m_roomRightDownLeft);
                }
                else
                {
                    tempRoom = Instantiate(m_RoomsList.m_roomRightDown[
                            Random.Range(0, m_RoomsList.m_roomRightDown.Length-1)], transform.position, Quaternion.identity);
                    //InstantiateRoom(m_RoomsList.m_roomRightDown);
                }
            }
            else if (left)
            {
                tempRoom = Instantiate(m_RoomsList.m_roomDownLeft[
                            Random.Range(0, m_RoomsList.m_roomDownLeft.Length-1)], transform.position, Quaternion.identity);
                //InstantiateRoom(m_RoomsList.m_roomDownLeft);
            }
            else
            {
                tempRoom = Instantiate(m_RoomsList.m_roomDown[
                           Random.Range(0, m_RoomsList.m_roomDown.Length-1)], transform.position, Quaternion.identity);
                //InstantiateRoom(m_RoomsList.m_roomDown);
            }

            return;
        }
        if (right)
        {
            if (left)
            {
                tempRoom = Instantiate(m_RoomsList.m_roomRightLeft[
                            Random.Range(0, m_RoomsList.m_roomRightLeft.Length-1)], transform.position, Quaternion.identity);
                //InstantiateRoom(m_RoomsList.m_roomRightLeft);
            }
            else
            {
                tempRoom = Instantiate(m_RoomsList.m_roomRight[
                            Random.Range(0, m_RoomsList.m_roomRight.Length-1)], transform.position, Quaternion.identity);
                //InstantiateRoom(m_RoomsList.m_roomRight);
            }
        }
        else
        {
            tempRoom = Instantiate(m_RoomsList.m_roomLeft[
                Random.Range(0, m_RoomsList.m_roomLeft.Length-1)], transform.position, Quaternion.identity);
            //InstantiateRoom(m_RoomsList.m_roomLeft);
        }


        if (tempRoom != null)
            tempRoom.transform.SetParent(m_RoomParent);
    }
}
