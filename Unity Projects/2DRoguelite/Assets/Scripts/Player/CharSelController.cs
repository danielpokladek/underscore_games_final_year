using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelController : PlayerController
{
    [SerializeField] private GameObject playerPrefab;
    
    override protected void Start()
    {
        base.Start();

        canMove = false;
    }

    private void OnMouseDown()
    {
        canMove = true;
        GameManager.current.playerPrefab = playerPrefab;
    }

    override protected void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.H))
            LevelManager.instance.StartGame();
    }
}
