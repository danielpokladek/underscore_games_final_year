using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] protected int itemPrice;
    [SerializeField] protected string itemName;

    private bool playerInRange;

    virtual public void Interact(PlayerController playerController)
    {
        Debug.Log("It just works! - Todd Howard");
    }

    protected void PurchaseItem(int gemAmount)
    {
        GameManager.current.GetPlayerGems -= gemAmount;
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

        if ((GameManager.current.GetPlayerGems - itemPrice) >= 0)
            return true;

        Debug.Log("No money, no buy!", gameObject);
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIManager.currentInstance.ShowItemUI(transform.position, itemName, itemPrice);
            gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 5.0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIManager.currentInstance.HideItemUI();
            gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineThickness", 0.0f);
        }
    }
}
