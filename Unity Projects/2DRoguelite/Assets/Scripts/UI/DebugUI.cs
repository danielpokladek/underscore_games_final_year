using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    GameManager gameManager;
    LevelManager levelManager;
    PlayerController player;

    private bool showUI = false;

    private void Start()
    {
        gameManager = GameManager.current;
        levelManager = LevelManager.instance;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Day State: " + levelManager.GetCurrentState);
        GUI.Label(new Rect(10, 25, 200, 20), "State Timer: " + levelManager.GetStateTimer.ToString("00.00"));
    }
}
