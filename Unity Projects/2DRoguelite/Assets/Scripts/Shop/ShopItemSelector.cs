using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemSelector : MonoBehaviour
{
    [SerializeField] public ItemContainer itemContainer;
    [SerializeField] public GameObject[] itemsList;
    [SerializeField] public GameObject[] itemPlaces;

    void Start()
    {
        foreach (GameObject _go in itemPlaces)
        {
            Item tempItem = itemContainer.GetItem();
            GameObject tempShopObject = Instantiate(tempItem.item, _go.transform.position, Quaternion.identity);

            tempShopObject.GetComponent<InteractableItem>().Item(tempItem.itemName, tempItem.itemPrice, false);
        }
    }
}
