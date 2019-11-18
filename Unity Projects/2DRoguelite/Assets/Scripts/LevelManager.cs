using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum DayState { Day, Night, Midnight, Boss };

    [SerializeField] public DayState currentState;
    [SerializeField] private float dayLength;
    [SerializeField] private float nightLength;
    [SerializeField] private float midnightLength;

    private string currentStateString;
    private float stateTimer;

    public static LevelManager instance = null;

    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentState       = DayState.Day;
        currentStateString = "Day";
    }

    private void Update()
    {
        UpdateDayState();

        if (Input.GetKeyDown(KeyCode.F5))
            LoadBossBattle();
    }

    private void LoadBossBattle()
    {
        SceneManager.LoadScene(1);
    }

    private void UpdateDayState()
    {
        switch (currentState)
        {
            case DayState.Boss:
                break;
            
            // Handle Day.
            case DayState.Day:
                if (stateTimer >= dayLength)
                {
                    // Day is up, spooky time!
                    currentStateString = "Night";
                    SetState(DayState.Night);
                }
                else
                {
                    stateTimer += Time.deltaTime;
                }
                break;

            // Handle Night.
            case DayState.Night:
                if (stateTimer >= nightLength)
                {
                    // Night is up, the blood moon is upon you!... or something
                    currentStateString = "Midnight";
                    SetState(DayState.Midnight);
                }
                else
                {
                    stateTimer += Time.deltaTime;
                }
                break;

            // Handle Midnight.
            case DayState.Midnight:
                if (stateTimer >= midnightLength)
                {
                    // Midnight is up, time to chill out!
                    currentStateString = "Day";
                    SetState(DayState.Day);
                }
                else
                {
                    stateTimer += Time.deltaTime;
                }
                break;
        }
    }

    private void SetState(DayState state)
    {
        currentState = state;
        stateTimer = 0;
    }

    public string GetCurrentState { get { return currentStateString; } }
    public float GetStateTimer { get { return stateTimer; } }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
