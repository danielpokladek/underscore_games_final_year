using UnityEngine;
using TMPro;

public class ItemPickupUI : MonoBehaviour
{
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDesc;

    private void Start()
    {
        HideUI();
    }

    public void ShowUI(string itemName, string itemDesc)
    {
        this.itemName.text = itemName;
        this.itemDesc.text = itemDesc;

        itemPanel.SetActive(true);
    }

    public void HideUI()
    {
        itemPanel.SetActive(false);
    }
}
