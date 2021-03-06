﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BossPortal : MonoBehaviour
{
    [SerializeField] private TMP_Text soulsText;
    [SerializeField] private Animator portalAnimator;

    [SerializeField] private Material innerMaterial;

    private bool portalEnabled = false;
    private bool showInner;

    private float dissolveAmount;

    private void Start()
    {
        GameManager.current.bossPortalRef = gameObject;
        LevelManager.instance.portalChargedCallback += EnablePortal;
        portalAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        soulsText.text = LevelManager.instance.killsRequired.ToString("00");

        if (showInner && dissolveAmount < 1)
        {
            dissolveAmount += Time.deltaTime * 1.5f;
            innerMaterial.SetFloat("_DissolveAmount", dissolveAmount);
        }
    }

    public void ShowInnerPortal()
    {
        dissolveAmount = 0;
        showInner = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (portalEnabled)
                GameManager.current.NextLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void EnablePortal()
    {
        portalEnabled = true;
        soulsText.enabled = false;
        portalAnimator.SetTrigger("_OpenPortal");
    }
}
