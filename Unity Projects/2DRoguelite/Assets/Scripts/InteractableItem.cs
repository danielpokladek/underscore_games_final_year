using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    // Is equal to true when the players is in range of an item.
    private bool playerInRange;

    virtual public void Interact()
    {
        Debug.Log("It works!");
    }

    public bool PlayerInRange { get; set; }
}
