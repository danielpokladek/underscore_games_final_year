using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public int itemPrice;
    public GameObject item;
    public float probability;
}

[CreateAssetMenu(fileName = "Item Container", menuName = "Item Container")]
public class ItemContainer : ScriptableObject
{
    public Item[] itemsList;

    public Item GetItem()
    {
        float totalProbability = 0;

        foreach (Item item in itemsList)
        {
            totalProbability += item.probability;
        }

        float randomPoint = Random.value * totalProbability;

        for (int i = 0; i < itemsList.Length; i++)
        {
            if (randomPoint < itemsList[i].probability)
            {
                return itemsList[i];
            }
            else
            {
                randomPoint -= itemsList[i].probability;
            }
        }

        return itemsList[itemsList.Length -1];
    }
}
