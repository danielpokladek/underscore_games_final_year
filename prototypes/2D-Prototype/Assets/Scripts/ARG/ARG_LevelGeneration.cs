﻿using System.Collections;
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
    [Header("Level Generation Settings")]
    [Tooltip("This is the prefab of a blank room, from this object the generator will create rooms and assign appropriate parameters." +
        "This should only be assigned to the prefab used in generation, and anything else should be adjusted in the script.")]
    public GameObject roomWhiteObj;

    [Tooltip("This is the size of the grid in halfs, so for example by default, if set to (4,4) this will create an 8x8 grid of rooms." +
        "This can be adjusted on the go, and can be used to generate bigger grids later on in the game.")]
    public Vector2 worldSize = new Vector2(4, 4);

    [Tooltip("This is the number of rooms we want in the level, but make sure not to exceed the max amount of rooms possible in the grid." +
        "Adjusting this number higher, will naturally create more rooms and vice versa when adjusting this number lower." +
        "This can be used to create more rooms later on in the game, when we want the players to spend more time on one level." +
        "This ideally should be used together with 'worldSize' to create bigger maps for players to play on.")]
    public int numberOfRooms = 20;

    public Transform mapRoot;

    ARG_Room[,] rooms;                                      // Reference to a 2D array of rooms in the level.

    List<Vector2> takenPositions = new List<Vector2>();     // Although we already have an array of rooms,
                                                            //  a List<> allows to find rooms using 'contains' method.

    private int gridSizeX, gridSizeY;

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
                } while (NumberOfNeighbours(checkPos, takenPositions) > 1 && iterations < 100);

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
        int x = 0;
        int y = 0;
        Vector2 checkingPos = Vector2.zero;

        do
        {
            // Grab a random taken position from the list,
            //  and select if going up/down, left/right.
            // Keep going until position is not already taken,
            //  and make sure it is inside our grid (within room boundaries).

            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;

            bool UpDown = (Random.value < .5f);
            bool positive = (Random.value < .5f);

            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }

            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }

    private Vector2 SelectiveNewPosition()
    {
        int index = 0;
        int inc = 0;
        int x = 0;
        int y = 0;
        Vector2 checkingPos = Vector2.zero;

        do
        {
            // Find a room which only has one neighbour.
            // This fucntion will add to the branching effect, whenever wanted.

            inc = 0;
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbours(takenPositions[index], takenPositions) > 1 && inc < 100);

            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < .5f);
            bool positive = (Random.value < .5f);

            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }

            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

        if (inc >= 100)
        {
            Debug.LogError("GEN_ERROR: Could not find position with only one neighbour.");
        }

        return checkingPos;
    }

    private int NumberOfNeighbours(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        // Start with a zero (no neighbours), and increment the number each time a neighbour is found.

        int retValue = 0;

        if (usedPositions.Contains(checkingPos + Vector2.right))
            retValue++;

        if (usedPositions.Contains(checkingPos + Vector2.left))
            retValue++;

        if (usedPositions.Contains(checkingPos + Vector2.up))
            retValue++;

        if (usedPositions.Contains(checkingPos + Vector2.down))
            retValue++;

        return retValue;
    }

    /// <summary>
    /// Loop through all rooms in the level, checking if they have neighbours, and select which doors they need.
    /// </summary>
    private void SetRoomDoors()
    {
        // Use a nested for loop to check each slot in the grid, to check for neighbours of the room.
        //  If the room does indeed have neighbours, select the appropriate door and move to the next room.

        for (int x = 0; x < ((gridSizeX * 2)); x++)
        {
            for (int y = 0; y < ((gridSizeY * 2)); y++)
            {
                // If there is no room in this grid slot, move on.
                if (rooms[x, y] == null)
                    continue;

                Vector2 gridPos = new Vector2(x, y);

                // Check above
                if (y - 1 < 0)
                    rooms[x, y].doorBot = false;
                else
                    rooms[x, y].doorBot = (rooms[x, y - 1] != null);

                // Check below
                if (y + 1 >= gridSizeY * 2)
                    rooms[x, y].doorTop = false;
                else
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);

                // Check left
                if (x - 1 < 0)
                    rooms[x, y].doorLeft = false;
                else
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);

                // Check right
                if (x + 1 >= gridSizeX * 2)
                    rooms[x, y].doorRight = false;
                else
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
            }
        }
    }

    private void DrawMap()
    {
        // Loop through each room on the gird,
        //  multiply by size of room,
        //  instantiate and assign variables.
        foreach (ARG_Room room in rooms)
        {
            if (room == null)
                continue;

            Vector2 drawPos = room.gridPos;

            // Ideally allow users to select the size of the room in generation script,
            //  this will allow designers to mess about with this without adjusting the code.
            drawPos.x *= 16;
            drawPos.y *= 8;

            ARG_SpriteSelector selector = Object.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<ARG_SpriteSelector>();

            selector.type   = room.roomType;
            selector.up     = room.doorTop;
            selector.down   = room.doorBot;
            selector.right  = room.doorRight;
            selector.left   = room.doorLeft;

            selector.gameObject.transform.parent = mapRoot;
        }
    }
}
