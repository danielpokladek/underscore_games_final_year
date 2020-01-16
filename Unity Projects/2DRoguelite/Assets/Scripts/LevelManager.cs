using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    [SerializeField] private float dayLength;
    [SerializeField] private float nightLength;
    [SerializeField] private float midnightLength;

    public GameObject portalPrefab;
    public GameObject shopPrefab;

    public GameObject minimapBoss;
    public GameObject minimapShop;

    // -------------------------------------------------
    public enum DayState { PlayerSel, Day, Night, Midnight, Boss };
    public DayState currentState;

    // -------------------------------------
    public delegate void OnDayStateChange();
    public OnDayStateChange onDayStateChangeCallback;

    public GameObject playerPrefab;

    private string currentStateString = "N/A";
    private float stateTimer;

    private void Start()
    {
        if (currentState == DayState.PlayerSel || currentState == DayState.Boss)
            return;

        currentState       = DayState.Day;
        currentStateString = "Day";
    }

    public void LoadBossBattle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        UpdateDayState();

        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.F5))
                LoadBossBattle();

            if (Input.GetKeyDown(KeyCode.Backspace))
                Restart();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SavePlayerStats();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void SavePlayerStats()
    {
        if (GameManager.current.levelCounter > 0)
            SaveManager.current.Save();
    }

    public void LoadPlayerStats()
    {
        if (GameManager.current.levelCounter > 0)
            SaveManager.current.Load();
    }

    private void UpdateDayState()
    {
        switch (currentState)
        {
            case DayState.PlayerSel:
                break;
            
            case DayState.Boss:
                // No day/night transition in the boss battle stage.
                break;
            
            // Handle Day.
            case DayState.Day:
                if (stateTimer >= dayLength)
                    SetState(DayState.Night, "Night");
                else
                    stateTimer += Time.deltaTime;
                break;

            // Handle Night.
            case DayState.Night:
                if (stateTimer >= nightLength)
                    SetState(DayState.Midnight, "Midnight");
                else
                    stateTimer += Time.deltaTime;

                break;

            // Handle Midnight.
            case DayState.Midnight:
                if (stateTimer >= midnightLength)
                    SetState(DayState.Day, "Day");
                else
                    stateTimer += Time.deltaTime;

                break;
        }
    }

    private void SetState(DayState state, string stateString)
    {
        currentStateString = stateString;
        currentState       = state;
        stateTimer         = 0;

        if (onDayStateChangeCallback != null)
            onDayStateChangeCallback.Invoke();
    }

    public string GetCurrentState { get { return currentStateString; } }
    public float GetStateTimer { get { return stateTimer; } }

    public void LevelComplete()
    {
        GameManager.current.levelCounter += 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
