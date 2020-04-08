using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [System.Serializable]
    public class Effects
    {
        public GameObject damageIndicator;
        public GameObject healIndicator;
    }

    public Canvas gameCanvas;
    public GameObject priceUIPrefab;
    [SerializeField] private GameObject loadingScreen;

    private GameObject priceUIRef;
    private ItemUIController itemUIController;

    public Effects effectsContainer;
    public static GameUIManager currentInstance;

    private void Awake()
    {
        if (currentInstance == null)
            currentInstance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        priceUIRef = Instantiate(priceUIPrefab, transform.position, Quaternion.identity);
        itemUIController = priceUIRef.GetComponent<ItemUIController>();

        HideItemUI();
    }

    public void DamageIndicator(Vector2 position, float damageAmount)
    {      
        GameObject text = Instantiate(effectsContainer.damageIndicator);

        text.transform.SetParent(gameCanvas.transform);

        string damageText = "-" + damageAmount;
        text.GetComponent<IndicatorText>().SetValues(damageText, position);
    }

    public void HealIndicator(Vector2 position, float healAmount)
    {
        GameObject text = Instantiate(effectsContainer.healIndicator);

        text.transform.parent = gameCanvas.transform;

        string healText = "+" + healAmount;
        text.GetComponent<IndicatorText>().SetValues(healText, position);
    }

    public void ShowItemUI(Vector2 _worldPosition, string _itemName, int _itemPrice, Color textColor, bool _isDungeonItem = false)
    {
        priceUIRef.transform.position = _worldPosition;
        itemUIController.SetValues(_itemName, _itemPrice, textColor, _isDungeonItem);
    }

    public void HideItemUI()
    {
        itemUIController.Hide();
    }
}
