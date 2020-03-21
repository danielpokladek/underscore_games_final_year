using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemPriceText;
    [SerializeField] private Image gemIcon;

    public void SetValues(string _itemName, int _itemPrice, bool _dungeonChest = false)
    {
        if (!_dungeonChest)
        {
            itemNameText.text  = _itemName + ":";
            itemPriceText.text = _itemPrice.ToString("");

            gemIcon.enabled = true;
            itemPriceText.enabled = true;
        }
        else
        {
            itemNameText.text  = _itemName;
            itemPriceText.text = "0";

            gemIcon.enabled = false;
            itemPriceText.enabled = false;
        }
    }
}
