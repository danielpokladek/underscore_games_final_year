using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Level Generation Script
 * 
 * Description:
 * Not done.
 * 
 */

public class ARG_LevelGeneration : MonoBehaviour
{
    public GameObject roomWhiteObj;

    Vector2 worldSize = new Vector2(4, 4);                  // This is the size of the world (in room lenghts),
                                                            //  but this in halfs, so (4,4) will create an 8x8 grid.

    ARG_Room[,] rooms;                                      // Reference to a 2D array of rooms in the level.

    List<Vector2> takenPositions = new List<Vector2>();     // Although we already have an array of rooms,
                                                            //  a List<> allows to find rooms using 'contains' method.

    private int gridSizeX, gridSizeY;
    private int numberOfRooms = 20;

    private void Start()
    {
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }

        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);

        CreateRooms();
        SetRoomDoors();
        DrawMap();
    }

    private void CreateRooms()
    {
        throw new System.NotImplementedException();
    }

    private void SetRoomDoors()
    {
        throw new System.NotImplementedException();
    }

    private void DrawMap()
    {
        throw new System.NotImplementedException();
    }
}
