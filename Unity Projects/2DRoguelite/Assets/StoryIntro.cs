using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryIntro : MonoBehaviour
{
    [SerializeField] private Sprite[] storyImages;
    [SerializeField] private CanvasGroup storyCanvas;
    [SerializeField] private Image storyCanvasImage;

    int iterator = 0;

    private void Start()
    {
        storyCanvasImage.sprite = storyImages[0];

        StartCoroutine(FadeInCanvas());
    }

    private void Update()
    {
        if (Input.GetButtonDown("LMB"))
            NextImage();

        if (Input.GetKeyDown(KeyCode.Escape))
            LoadMenu();
    }

    public void NextImage()
    {
        iterator += 1;

        if (iterator > storyImages.Length - 1)
        {
            StartCoroutine(LoadMenu());
        }
        else
        {
            storyCanvasImage.sprite = storyImages[iterator];
        }
    }

    private IEnumerator LoadMenu()
    {
        for (float t = 0.01f; t < 1.0f; t += Time.deltaTime)
        {
            storyCanvas.alpha = Mathf.Lerp(1, 0, Mathf.Min(1, t / 1.0f));
            yield return null;
        }

        GameManager.current.LoadScene((int)SceneIndexes.STORY, (int)SceneIndexes.MENU);
    }

    private IEnumerator FadeInCanvas()
    {
        for (float t = 0.01f; t < 2.0f; t += Time.deltaTime)
        {
            storyCanvas.alpha = Mathf.Lerp(0, 1, Mathf.Min(1, t / 2.0f));
            yield return null;
        }

        yield return new WaitForSeconds(5.0f);
    }
}
