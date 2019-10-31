using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempUIManager : MonoBehaviour
{
    [SerializeField] private GameObject bossObject;
    [SerializeField] private TMP_Text healthText;

    private BossController bossController;

    private void Start()
    {
        bossController = bossObject.GetComponent<BossController>();
    }

    private void Update()
    {
        healthText.text = bossController.GetHealth().ToString();
    }
}
