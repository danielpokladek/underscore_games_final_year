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
            SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
    }
    #endregion

    [SerializeField] private GameObject loadingScreen;

    public int enemyKilled;
    public int gemsCollected;

    public int levelCounter = 0;

    public GameObject playerPrefab;
    public GameObject bossPortalRef;
    
    [SerializeField] private int playerGems;

    [HideInInspector] public GameObject playerRef;

    public delegate void LoadingFinished();
    public LoadingFinished loadingFinishedCallback;

    private void Start()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    public int PlayerGems { get { return playerGems; } set { playerGems = value; } }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    Scene _sceneToLoad;

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }

    public void LoadTutorial()
    {
        loadingScreen.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TUTORIAL, LoadSceneMode.Additive));

        _sceneToLoad = SceneManager.GetSceneByBuildIndex((int)SceneIndexes.TUTORIAL);

        StartCoroutine(GetLoadProgress());
    }

    public void LoadMain()
    {
        loadingScreen.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN, LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));

        StartCoroutine(GetLoadProgress());
    }

    public void LoadScene(int sceneToUnload, int sceneToLoad)
    {
        loadingScreen.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync(sceneToUnload));
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive));

        StartCoroutine(GetLoadProgress());
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