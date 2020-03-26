using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScriptThing : MonoBehaviour
{
    private void Start()
    {
        GameManager.current.gemsCollected += 1000;
    }
}
