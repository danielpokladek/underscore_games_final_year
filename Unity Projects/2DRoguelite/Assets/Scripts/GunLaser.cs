using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLaser : MonoBehaviour
{
    [SerializeField] private Transform laserHit;
    [SerializeField] private Transform laserPos;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled          = false;
        lr.useWorldSpace    = true;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(laserPos.position, laserPos.up);
        laserHit.position = hit.point;

        lr.SetPosition(0, laserPos.position);
        lr.SetPosition(1, laserHit.position);

        if (Input.GetKeyDown(KeyCode.T))
        {
            lr.enabled = !lr.enabled;
        }
    }
}
