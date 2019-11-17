using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [System.Serializable]
    public class Effects
    {
        public GameObject damageIndicator;
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

        text.transform.parent = gameCanvas.transform;
        text.GetComponent<IndicatorText>().SetValues(damageAmount.ToString(), position);
    }
}
