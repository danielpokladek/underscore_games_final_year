using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using UnityEditorInternal;

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
    [SerializeField] private float dawnStateLength;
    [SerializeField] private float dayStateLength;
    [SerializeField] private float duskStateLength;
    [SerializeField] private float nightStateLength;

    public enum DayState
    {
        PlayerSel,
        Dawn,
        Day,
        Dusk,
        Night,
        Boss 
    };
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
    [SerializeField] private Gradient dayGradient;
    [Tooltip("Minimum sun intensity, this will be used at night; lower this value to create darker night. " +
        "Avoid making this value too low, as it can cause the light to become too dark and result in a black scene.")]
    [SerializeField] private float minLightIntensity = 0.7f;
    [Tooltip("Maximum sun intensity, this will be used at day; increase this value to create brighter day. " +
        "Avoid making this value too high, as it can cause the light to become too bright and result in white scene.")]
    [SerializeField] private float maxLightIntensity = 1.0f;
    [SerializeField] private float sunFade = 3.0f;

    [Header("Music Settings")]
    [SerializeField] private AudioClip dayMusic;
    [SerializeField] private AudioClip nightMusic;
    [SerializeField] private AudioClip caveMusic;
    [SerializeField] private AudioClip bossStage;
    [SerializeField] private AudioClip deathMusic;

    // -------------------------------------
    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs : EventArgs
    {
        public float stateLength;
    }

    public delegate void OnEnemyKilled();
    public OnEnemyKilled onEnemyKilledCallback;

    public delegate void OnDayStateChange();
    public OnDayStateChange onDayStateChangeCallback;

    public delegate void PortalCharged();
    public PortalCharged portalChargedCallback;

    [HideInInspector] public bool countTime = true;
    [HideInInspector] public bool playerDead = false;
    [HideInInspector] public float dayLength;
    [HideInInspector] public float dayTimer;

    private string currentStateString = "N/A";
    private float stateTimer;
    private float stateLength;

    private bool inDungeon;

    // Day to night (& vice versa) fade times;
    private float d2nFade;
    private float n2dFade;
    private float fadeTimer;

    private bool gameTime = true;

    [HideInInspector] public float enemyKills { get; private set; }

    private void Start()
    {
        if (currentState == DayState.Boss)
        {
            AudioManager.current.CrossFadeMusicClips(bossStage);
            return;
        }

        AudioManager.current.PlayMusic(dayMusic);
        dayLength = dawnStateLength + dayStateLength + duskStateLength + nightStateLength;

        OnStateChange?.Invoke(
            this, new OnStateChangeEventArgs { stateLength = dayStateLength });
    }

    private void Update()
    {
        UpdateDayState();

        // Probably should grab the time when level started,
        //  and deduct the time when level finished to get better performance.
        //  But will do for now, unless game grinds to halt.
        if (gameTime && !PauseMenu.GameIsPaused)
            GameManager.current.GameTime += Time.deltaTime;
    }

    public void PlayCaveMusic()
    {
        AudioManager.current.CrossFadeMusicClips(caveMusic, .5f);
        inDungeon = true;
    }

    public void PlayNormalMusic()
    {
        inDungeon = false;

        if (currentState == DayState.Dawn)
            AudioManager.current.CrossFadeMusicClips(dayMusic, .5f);
        else if (currentState == DayState.Dusk)
            AudioManager.current.CrossFadeMusicClips(nightMusic, .5f);
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

    private void UpdateDayState()
    {
        if (countTime)
        {
            dayTimer += Time.deltaTime;
            stateTimer += Time.deltaTime;
        }

        switch (currentState)
        {
            case DayState.PlayerSel:
                break;
            
            case DayState.Boss:
                break; 

            case DayState.Dawn:
                if (stateTimer >= dawnStateLength)
                {
                    SetState(DayState.Day, "Day");
                    OnStateChange?.Invoke(
                        this, new OnStateChangeEventArgs { stateLength = dayStateLength });
                }

                if (!enableSunProgress)
                    break;

                if (stateTimer <= dawnStateLength)
                {
                    fadeTimer += Time.deltaTime;
                    ambientLight.intensity = Mathf.Lerp(ambientLight.intensity, maxLightIntensity, (fadeTimer / dawnStateLength));
                    ambientLight.color = dayGradient.Evaluate(Mathf.Clamp(1 - (fadeTimer / dawnStateLength), 0, 1));
                }

                break;

            case DayState.Day:
                if (stateTimer > dayStateLength)
                {
                    SetState(DayState.Dusk, "Dusk");
                    OnStateChange?.Invoke(
                        this, new OnStateChangeEventArgs { stateLength = duskStateLength });
                }

                if (!enableSunProgress)
                    return;

                ambientLight.intensity = maxLightIntensity;
                ambientLight.color = dayGradient.Evaluate(0);

                break;


            case DayState.Dusk:
                if (stateTimer >= duskStateLength)
                {
                    SetState(DayState.Night, "Night");
                    OnStateChange?.Invoke(
                        this, new OnStateChangeEventArgs { stateLength = nightStateLength });
                }

                if (!enableSunProgress)
                    break;

                if (stateTimer <= duskStateLength)
                {
                    fadeTimer += Time.deltaTime;
                    ambientLight.intensity = Mathf.Lerp(ambientLight.intensity, minLightIntensity, (fadeTimer / duskStateLength));
                    ambientLight.color = dayGradient.Evaluate(fadeTimer / duskStateLength);
                }

                break;

            case DayState.Night:
                if (stateTimer >= nightStateLength)
                {
                    SetState(DayState.Dawn, "Dawn");
                    OnStateChange?.Invoke(
                        this, new OnStateChangeEventArgs { stateLength = dawnStateLength });
                }

                if (!enableSunProgress)
                    return;

                ambientLight.intensity = minLightIntensity;
                ambientLight.color = dayGradient.Evaluate(1);

                break;
        }
    }

    private void SetState(DayState state, string stateString)
    {
        currentStateString = stateString;
        currentState       = state;
        stateTimer         = 0;
        fadeTimer          = 0;

        onDayStateChangeCallback?.Invoke();

        if (inDungeon)
            return;

        if (state == DayState.Dawn && !LevelManager.instance.playerDead)
            AudioManager.current.CrossFadeMusicClips(dayMusic);
        else if (state == DayState.Dusk && !LevelManager.instance.playerDead)
            AudioManager.current.CrossFadeMusicClips(nightMusic);
    }

    public string GetCurrentState { get { return currentStateString; } }
    public float GetStateTimer { get { return stateTimer; } }
}
