using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnStart : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;

    private void Start()
    {
        GameManager.current.LoadScene(SceneManager.GetActiveScene().buildIndex, sceneToLoad);
    }
}
