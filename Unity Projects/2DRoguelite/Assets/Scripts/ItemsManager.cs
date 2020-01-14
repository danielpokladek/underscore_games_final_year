using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    #region Singleton
    public static ItemsManager current = null;
    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);
    }
    #endregion

    public ItemPool tempChestItems;
}
