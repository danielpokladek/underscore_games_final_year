using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemSelector : MonoBehaviour
{
    [SerializeField] public ShopItemsContainer itemContainer;
    [SerializeField] public GameObject[] itemsList;
    [SerializeField] public GameObject[] itemPlaces;

    void Start()
    {
        foreach (GameObject _go in itemPlaces)
        {
            //int rand = Random.Range(0, itemsList.Length);

            GameObject tempShopObject = itemContainer.GetShopItem();

            Instantiate(tempShopObject, _go.transform.position, Quaternion.identity);
        }
    }
}
