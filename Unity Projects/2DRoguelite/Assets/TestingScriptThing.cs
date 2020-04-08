using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScriptThing : MonoBehaviour
{
    private void Start()
    {
        GameManager.current.PlayerGems += 1000;
    }
}
