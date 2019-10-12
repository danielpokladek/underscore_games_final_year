using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Level Generation Script
 * 
 * Description:
 * Not done.
 * 
 * Room Types List:
 *  -   0: Normal Room
 *  -   1: (starting) Entrance Room
 *  -   2: (boss) Final Room
 *  -   3: Shop Room
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
        rooms = new ARG_Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new ARG_Room(Vector2.zero, 1);
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;

        // Magic Numbers used in branching the rooms.
        float randomCompare = 0.2f;
        float randomCompareStart = 0.2f;
        float randomCompareEnd = 0.01f;

        // Add Rooms
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            // This generator will determine wether the rooms should be clumped together, or branched out.
            // The randomCompare also will make it, so that the more rooms there are, the less chance for the level to branch out.
            // Changing the magic numbers, will allow for more/less clumped levels.

            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

            // Grab New Room Position
            checkPos = NewPosition();

            // Test New Position
            if (NumberOfNeighbours(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                // If we do want to branch out, the generator will look for a new position with 1 neighbour that connects it.
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeigbours(checkPos, takenPositions) > 1 && iterations < 100);

                if (iterations >= 50)
                    Debug.LogError("GEN_ERROR: Could not create with fewer neighbours than: " + NumberOfNeighbours(checkPos, takenPositions));
            }

            // Finalize Position, add to list and calculate the offset.
            // Once it is finalized, construct the new room position with roomType '0' (normal room - check above for room types)
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new ARG_Room(checkPos, 0);
            takenPositions.Insert(0, checkPos);
        }
    }

    private Vector2 NewPosition()
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
