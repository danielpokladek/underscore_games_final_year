using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public TMPro.TMP_Text itemText;

    public void SetText(string text)
    {
        itemText.text = text;
    }
}
