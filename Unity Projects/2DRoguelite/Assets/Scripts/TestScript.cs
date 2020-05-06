using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Dictionary<GameObject, float> shopItems = new Dictionary<GameObject, float>();

    [Serializable]
    public struct Item
    {
        public GameObject item;
        public float probability;
    }

    public Item[] items;
}
