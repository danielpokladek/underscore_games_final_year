using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items Pool", menuName = "New Items Pool", order = 1)]
public class ItemPool : ScriptableObject
{
    public GameObject[] items;
}
