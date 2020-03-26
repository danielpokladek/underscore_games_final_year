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
        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);

        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
        loadingScreen.gameObject.SetActive(false);
    }
    #endregion

    [SerializeField] private GameObject loadingScreen;

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

    public int PlayerGems { get { return playerGems; } set { playerGems = value; } }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadTutorial()
    {
        loadingScreen.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TUTORIAL, LoadSceneMode.Additive));

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

        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.UnloadSceneAsync(sceneToUnload));

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneToLoad));

        StartCoroutine(GetLoadProgress());
    }

    private IEnumerator GetLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            yield return new WaitForEndOfFrame();

            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }

        loadingScreen.gameObject.SetActive(false);
    }
}