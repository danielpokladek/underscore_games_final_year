using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup splashCanvas;

    private void Start()
    {
        StartCoroutine(Splash());
    }

    private IEnumerator Splash()
    {
        for (float t = 0.01f; t < 1.0f; t += Time.deltaTime)
        {
            splashCanvas.alpha = Mathf.Lerp(0, 1, Mathf.Min(1, t / 1.0f));
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);

        for (float t = 0.1f; t < 1.0f; t += Time.deltaTime)
        {
            splashCanvas.alpha = Mathf.Lerp(1, 0, Mathf.Min(1, t / 1.0f));
            yield return null;
        }

        GameManager.current.LoadScene(SceneManager.GetActiveScene().buildIndex, (int)SceneIndexes.STORY);
    }
}
