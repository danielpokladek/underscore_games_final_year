using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMasterItemList", menuName = "Master Item List")]
public class MasterItemList : ScriptableObject
{
    public ShopPlayerModifier[] playerItems;
}
