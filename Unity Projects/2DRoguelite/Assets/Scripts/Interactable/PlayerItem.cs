using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ideally I should create a Unity tool which allows designers to create new items,
//  and then automatically creates this file, but due to time limitation it will be left out.
//  Something to consider for future, or own project.

/// <summary>
/// Class used by PlayerController to keep track of what items players have collected.
/// </summary>
[CreateAssetMenu(fileName = "PlayerItem", menuName = "Create Player Item")]
public class PlayerItem : ScriptableObject
{
    [HideInInspector] public string itemName;
    [HideInInspector] public string itemDesc;
    [HideInInspector] public int itemID;

    public void SetItem(string name, string desc, int ID)
    {
        itemName = name;
        itemDesc = desc;
        itemID = ID;
    }
}
