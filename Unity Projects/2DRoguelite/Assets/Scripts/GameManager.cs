﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager current = null;
    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    public int enemyKilled;
    public int gemsCollected;

    public int levelCounter = 0;

    public GameObject playerPrefab;
    public GameObject playerRef;
    public GameObject bossPortalRef;
    
    [SerializeField] private int playerGems;


    public delegate void LoadingFinished();
    public LoadingFinished loadingFinishedCallback;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    loadingFinishedCallback.Invoke();
    }

    public int PlayerGems { get { return playerGems; } set { playerGems = value; } }
}
