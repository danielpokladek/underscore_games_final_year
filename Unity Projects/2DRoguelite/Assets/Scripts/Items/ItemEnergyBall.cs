using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEnergyBall : MonoBehaviour
{
    public Transform playerTrans;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(playerTrans.position, new Vector3(0, 0, 1), Time.deltaTime * 90);
    }
}
