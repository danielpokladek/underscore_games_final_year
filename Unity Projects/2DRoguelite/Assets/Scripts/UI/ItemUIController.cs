using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text itemNameText;
    [SerializeField] private TMPro.TMP_Text itemPriceText;

    private string itemName;
    private int itemPrice;

    public void SetValues(string _itemName, int _itemPrice)
    {
        itemNameText.text = _itemName + ": ";
        itemPriceText.text = _itemPrice.ToString("");
    }
}
