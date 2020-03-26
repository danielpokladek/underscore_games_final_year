using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMPro.TMP_Text theText;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
            theText.color = button.colors.highlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
            theText.color = button.colors.normalColor;
    }
}
