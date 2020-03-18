using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public GameObject item;
    public float probability;
}

[CreateAssetMenu(fileName = "Shop Items", menuName = "Shop Item Container")]
public class ShopItemsContainer : ScriptableObject
{
    public ShopItem[] shopItems;

    public GameObject GetShopItem()
    {
        float totalProbability = 0;

        foreach (ShopItem item in shopItems)
        {
            totalProbability += item.probability;
        }

        float randomPoint = Random.value * totalProbability;

        for (int i = 0; i < shopItems.Length; i++)
        {
            if (randomPoint < shopItems[i].probability)
            {
                return shopItems[i].item;
            }
            else
            {
                randomPoint -= shopItems[i].probability;
            }
        }

        return shopItems[shopItems.Length -1].item;
    }
}
