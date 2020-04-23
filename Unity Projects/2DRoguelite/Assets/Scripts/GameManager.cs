using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager current = null;
    private void Awake()
    {
        #if UNITY_EDITOR
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        #endif

        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        SceneManager.sceneLoaded += SceneLoaded;
        
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadSceneAsync((int)SceneIndexes.SPLASH, LoadSceneMode.Additive);
    }
    #endregion

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject loadingText;
    [SerializeField] private int playerGems;

    public GameObject playerPrefab;
    public MasterItemList masterItemList;

    public delegate void LoadingFinished();
    public LoadingFinished loadingFinishedCallback;

    private float gameTime;
    private float gameScore;
    private int enemyKilled;
    private int levelCounter = 0;

    [HideInInspector] public GameObject playerRef;
    [HideInInspector] public GameObject bossPortalRef;
    [HideInInspector] public bool enableLoadingText;
    public bool loadStats;

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public int LevelCount { get { return levelCounter; } }
    public int PlayerGems { get { return playerGems; } set { playerGems = value; } }
    public int EnemyCount { get { return enemyKilled; } set { enemyKilled = value; } }
    public float GameTime { get { return gameTime; } set { gameTime = value; } }
    public float GameScore { get { return gameScore; } set { gameScore = value; } }

    private void Start()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    public void NextLevel(int currentScene)
    {
        levelCounter += 1;

        SaveManager.current.Save();

        LoadScene(currentScene, currentScene + 1);
    }

    public void LoadScene(int sceneToUnload, int sceneToLoad)
    {
        if (enableLoadingText)
            loadingText.SetActive(true);
        else
            loadingText.SetActive(false);

        loadingScreen.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync(sceneToUnload));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive));


        StartCoroutine(GetLoadProgress());
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }

    private IEnumerator GetLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.1f);

        loadingScreen.gameObject.SetActive(false);
    }
}