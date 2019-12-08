using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] protected string itemName;
    [SerializeField] protected int    itemPrice;

    private bool showItemName;

    virtual public void Interact(PlayerController playerController) { }

    public void ShowName(bool value)
    {
        showItemName = value;
    }

    private void OnGUI()
    {
        if (showItemName)
            GUI.Label(new Rect(90, 10, 100, 20), "-" + itemPrice);
    }

    protected bool CheckGems(int _itemPrice)
    {
        if ((GameManager.current.PlayerCurrency - _itemPrice) > 0)
            return true;

        Debug.Log("Come back when you have some more gems");
        return false;
    }
}
