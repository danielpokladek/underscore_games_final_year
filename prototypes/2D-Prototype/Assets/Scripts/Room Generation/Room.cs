using UnityEngine;

public class Room
{
    public Vector2 gridPos;                                 // Room position on the grid.
    public int roomType;                                    // Room type (entrance, shop, boss, etc.)
    public bool doorTop, doorBot, doorLeft, doorRight;      // Which direction has doors (top, down, etc.)

    public Room(Vector2 _gridPos, int _roomType)
    {
        gridPos     = _gridPos;
        roomType    = _roomType;
    }
}
