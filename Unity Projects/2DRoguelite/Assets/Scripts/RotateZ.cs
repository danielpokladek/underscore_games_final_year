using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZ : MonoBehaviour
{
    private void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0, 0, 1), Random.Range(20, 25) * Time.deltaTime);
    }
}
