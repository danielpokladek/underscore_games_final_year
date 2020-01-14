using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonChest : InteractableItem
{
    private Animator anim;

    [Tooltip("RoomManager script of the dungeon.")]
    public LootRoom lootRoomManager;
    public Transform itemParent;

    private bool chestOpened;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    override public void Interact()
    {
        //Debug.Log("It just works!");

        if (!chestOpened)
            OpenChest();
    }

    private void OpenChest()
    {
        GameObject temp = ItemsManager.current.tempChestItems.items[Random.Range(0, ItemsManager.current.tempChestItems.items.Length)];
        GameObject item = Instantiate(temp, itemParent.position, Quaternion.identity);
        
        item.transform.SetParent(itemParent);
        lootRoomManager.ChestOpened();

        anim.SetTrigger("openChest");
        chestOpened = true;
    }
}
