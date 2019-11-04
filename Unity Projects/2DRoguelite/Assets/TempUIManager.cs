using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempUIManager : MonoBehaviour
{
    [SerializeField] private GameObject bossObject;
    [SerializeField] private TMP_Text bossHealth;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private TMP_Text playerHealth;

    private BossController bossController;
    private PlayerController playerController;

    private void Start()
    {
        bossController = bossObject.GetComponent<BossController>();
        playerController = playerObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        bossHealth.text = bossController.GetHealth().ToString();
        playerHealth.text = playerController.GetHealth.ToString();
    }
}
