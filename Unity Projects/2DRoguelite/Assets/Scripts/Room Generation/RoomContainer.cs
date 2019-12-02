using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room Container", menuName = "Room Generation", order = 0)]
public class RoomContainer : ScriptableObject
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
