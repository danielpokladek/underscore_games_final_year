﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] protected int itemPrice;
    [SerializeField] protected string itemName;
    [SerializeField] protected bool isDungeonItem = false;

    protected bool isDungeonChest;

    private bool playerInRange;

    public void Item(string _itemName, int _itemPrice, bool _isDungeonItem = false)
    {
        itemPrice = _itemPrice;
        itemName  = _itemName;
        isDungeonItem = _isDungeonItem;
    }

    virtual public void Interact(PlayerController playerController)
    {
        Debug.Log("It just works! - Todd Howard");
    }

    protected void PurchaseItem(int gemAmount)
    {
        GameManager.current.PlayerGems -= gemAmount;
        UIManager.current.updateUICallback.Invoke();
    }

    public bool PlayerInRange { get; set; }

    /// <summary>
    /// Check if player has enough gems to purchase the item.
    /// </summary>
    /// <returns>Returns true if player has enough gems to purchase the item.
    /// If price is '0', check is ignored and returns true.</returns>
    protected bool CheckGems()
    {
        if (itemPrice == 0)
            return true;

        if ((GameManager.current.PlayerGems - itemPrice) >= 0)
        {
            GameManager.current.PlayerGems -= itemPrice;
            UIManager.current.updateUICallback.Invoke();

            Debug.Log("Item purchased: " + itemName);
            return true;
        }

        Debug.Log("No money, no buy!", gameObject);
        return false;
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIManager.currentInstance.ShowItemUI(transform.position, itemName, itemPrice, isDungeonItem);
            gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 5.0f);
        }
    }

    virtual protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIManager.currentInstance.HideItemUI();
            gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0.0f);
        }
    }
}
