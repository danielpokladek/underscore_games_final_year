using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* !!THIS NEEDS RE-WRITING FOR BETTER UNDERSTANDING!!
 * 
 * ROOM  Script, contains data about the room
 *  
 * The Vector2 stores the current room position on the grid,
 * it's worth noting that not all grid slots are populated with rooms.
 * 
 * Room type determines the type of room this is, for example Entrance, Boss, Shop, etc.
 * 
 * Finally the bool determines which way the doors are facing.
 * This can probably be done with integer, or a char if required later.
 * Currently works fine for the rooms that we have.
 * 
 */

public class ARG_Room
{
                                                            // :-- EXPAND THIS INFORMATION LATER ON --:
                                                            //  
    public Vector2 gridPos;                                 // Room position on the grid.
    public int roomType;                                    // Room type (entrance, shop, boss, etc.)
    public bool doorTop, doorBot, doorLeft, doorRight;      // Which direction has doors (top, down, etc.)

    public ARG_Room(Vector2 _gridPos, int _roomType)
    {
        gridPos     = _gridPos;
        roomType    = _roomType;
    }
}
