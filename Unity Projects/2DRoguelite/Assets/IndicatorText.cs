using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorText : MonoBehaviour
{
    public Animator animator;
    public TMP_Text damageText;

    private Vector2 textPosition = new Vector2(0, 0);

    private void Start()
    {
        //animator    = GetComponent<Animator>();
        //damageText  = GetComponent<TMP_Text>();

        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
    }

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(textPosition);
    }

    public void SetValues(string text, Vector2 position)
    {
        damageText.text = "-" + text;
        textPosition = position;
    }
}
