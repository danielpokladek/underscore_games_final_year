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

    [SerializeField] private Canvas gameCanvas;

    public Effects effectsContainer;
    public static GameUIManager currentInstance;

    private void Awake()
    {
        if (currentInstance == null)
            currentInstance = this;
        else
            Destroy(gameObject);
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
}
