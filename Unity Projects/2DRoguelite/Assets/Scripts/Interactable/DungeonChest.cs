using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonChest : InteractableItem
{
    public ItemContainer itemContainer;

    private Animator anim;

    [Tooltip("RoomManager script of the dungeon.")]
    public LootRoom lootRoomManager;
    public Transform itemParent;

    private bool chestOpened;

    private void Start()
    {
        anim = GetComponent<Animator>();
        isDungeonChest = true;
    }

    override public void Interact(PlayerController playerController)
    {
        //Debug.Log("It just works!");

        if (!chestOpened)
            OpenChest();
    }

    private void OpenChest()
    {
        Item tempItem = itemContainer.GetItem();
        GameObject itemGO = Instantiate(tempItem.item, itemParent.position, Quaternion.identity);

        itemGO.GetComponent<InteractableItem>().Item(tempItem.itemName, tempItem.itemPrice, true);
        itemGO.transform.SetParent(itemParent);

        lootRoomManager.ChestOpened();
        anim.SetTrigger("openChest");
        chestOpened = true;

        //itemGO.GetComponent<Animator>().SetTrigger("gemJump");
    }

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!chestOpened)
            {
                GameUIManager.currentInstance.ShowItemUI(transform.position, itemName, itemPrice, isDungeonItem);
                gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 5.0f);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0.0f);
            }
        }
    }

    override protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!chestOpened)
            {
                GameUIManager.currentInstance.HideItemUI();
                gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0.0f);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0.0f);
            }
        }
    }
}
