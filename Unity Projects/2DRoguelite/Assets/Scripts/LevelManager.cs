using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

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

    [Header("State Settings")]
    [SerializeField] private float dayLength;
    [SerializeField] private float nightLength;
    [SerializeField] private float midnightLength;
    public enum DayState { PlayerSel, Day, Night, Midnight, Boss };
    public DayState currentState;

    public GameObject portalPrefab;
    public GameObject shopPrefab;

    public GameObject minimapBoss;
    public GameObject minimapShop;

    [Tooltip("Kills required for portal to work.")]
    public int killsRequired;

    [Header("Day & Night Visual Settings")]
    [Tooltip("Tick this box to enable the visual progress of the day and night cycle, " +
        "leaving this option disabled will still progress the time in game. " +
        "Brighter colours will result in higher sun intesity, and darker colours will result in lower intesity. " +
        "Use the gradient colour to adjust the brightness of the sun, as well as the colour.")]
    [SerializeField] private bool enableSunProgress;
    [Tooltip("Sun of the scene, this light will be used for the day and night cycle.")]
    [SerializeField] private Light2D  ambientLight;
    [Tooltip("Gradient which will be used on the progression from day to night." +
        "Start of the gradient will represent the daytime, and end of the gradient will represent night.")]
    [SerializeField] private Gradient dayToNightGradient;
    [Tooltip("The same as 'dayToNightGradient' but in this case the colours are reversed.")]
    [SerializeField] private Gradient nightToDayGradient;
    [Tooltip("Colour which will be used at night.")]
    [SerializeField] private Color nightColor;
    [Tooltip("Minimum sun intensity, this will be used at night; lower this value to create darker night. " +
        "Avoid making this value too low, as it can cause the light to become too dark and result in a black scene.")]
    [SerializeField] private float minLightIntensity = 0.7f;
    [Tooltip("Maximum sun intensity, this will be used at day; increase this value to create brighter day. " +
        "Avoid making this value too high, as it can cause the light to become too bright and result in white scene.")]
    [SerializeField] private float maxLightIntensity = 1.0f;

    [Header("Music Settings")]
    [SerializeField] private AudioClip dayMusic;
    [SerializeField] private AudioClip nightMusic;
    [SerializeField] private AudioClip bossStage;
    [SerializeField] private AudioClip deathMusic;

    // -------------------------------------
    public delegate void OnEnemyKilled();
    public OnEnemyKilled onEnemyKilledCallback;

    public delegate void OnDayStateChange();
    public OnDayStateChange onDayStateChangeCallback;

    public delegate void PortalCharged();
    public PortalCharged portalChargedCallback;

    [HideInInspector] public bool playerDead = false;

    private string currentStateString = "N/A";
    private float stateTimer;
    private float dayTimer;

    [HideInInspector] public float enemyKills { get; private set; }

    private void Start()
    {
        if (currentState == DayState.Boss)
        {
            AudioManager.current.CrossFadeMusicClips(bossStage);
            return;
        }

        currentState       = DayState.Day;
        currentStateString = "Day";

        AudioManager.current.PlayMusic(dayMusic);
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

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SavePlayerStats();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void AddSoul()
    {
        enemyKills++;

        if (enemyKills >= killsRequired)
        {
            if (portalChargedCallback != null)
                portalChargedCallback.Invoke();
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

                stateTimer += Time.deltaTime;
                dayTimer += Time.deltaTime;

                if (!enableSunProgress)
                    break;

                ambientLight.intensity = Mathf.Lerp(maxLightIntensity, minLightIntensity, (stateTimer / dayLength));
                ambientLight.color = dayToNightGradient.Evaluate(stateTimer / dayLength);

                break;

            // Handle Night.
            case DayState.Night:
                if (stateTimer >= nightLength)
                    SetState(DayState.Midnight, "Midnight");

                stateTimer += Time.deltaTime;

                if (!enableSunProgress)
                    break;

                ambientLight.intensity = minLightIntensity;
                ambientLight.color = nightColor;

                break;

            // Handle Midnight.
            case DayState.Midnight:
                if (stateTimer >= midnightLength)
                    SetState(DayState.Day, "Day");

                stateTimer += Time.deltaTime;

                if (!enableSunProgress)
                    break;

                ambientLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, (stateTimer / dayLength));
                ambientLight.color = nightToDayGradient.Evaluate(stateTimer / dayLength);

                break;
        }
    }

    private void SetState(DayState state, string stateString)
    {
        currentStateString = stateString;
        currentState       = state;
        stateTimer         = 0;

        if (state == DayState.Day && !LevelManager.instance.playerDead)
            AudioManager.current.CrossFadeMusicClips(dayMusic);
        else if (state == DayState.Night && !LevelManager.instance.playerDead)
            AudioManager.current.CrossFadeMusicClips(nightMusic);

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
