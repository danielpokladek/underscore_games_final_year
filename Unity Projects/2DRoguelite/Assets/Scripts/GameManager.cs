using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current = null;

    public GameObject playerPrefab;
    public GameObject playerRef;
    
    [HideInInspector] public GameObject bossPortalRef;
    
    [SerializeField] private int playerGems;
    
    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public int PlayerCurrency { get { return playerGems; } set { playerGems = value; } }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 80, 20), "Gems: " + playerGems);
    }
}
