using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemSelector : MonoBehaviour
{
    [SerializeField] public GameObject[] itemsList;
    [SerializeField] public GameObject[] itemPlaces;

    void Start()
    {
        foreach (GameObject _go in itemPlaces)
        {
            int rand = Random.Range(0, itemsList.Length);

            Instantiate(itemsList[rand], _go.transform.position, Quaternion.identity);
        }
    }
}
