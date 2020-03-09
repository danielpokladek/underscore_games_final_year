using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossPortal : MonoBehaviour
{
    [SerializeField] private TMP_Text portalCounter;

    private bool portalEnabled = false;

    private void Start()
    {
        LevelManager.instance.portalChargedCallback += EnablePortal;
    }

    private void Update()
    {
        portalCounter.text = LevelManager.instance.killsRequired.ToString("00");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (portalEnabled)
                LevelManager.instance.LoadBossBattle();
        }
    }

    private void EnablePortal()
    {
        portalEnabled = true;
    }
}
