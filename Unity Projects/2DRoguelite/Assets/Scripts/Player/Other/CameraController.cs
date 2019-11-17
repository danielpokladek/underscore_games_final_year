using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float maximumFollowSpeed   = 10f;
    [SerializeField] private float viewPortFactor       = .5f;
    [SerializeField] private float followDuration       = .1f;

    // --------------------------
    private Camera mainCamera;
    private Vector2 viewPortSize;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private Vector2 distance;

    private bool follow = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CameraInit());

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!follow)
            return;

        viewPortSize = (mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) - mainCamera.ScreenToWorldPoint(Vector2.zero)) * viewPortFactor;

        distance = playerTransform.position - transform.position;
        if (Mathf.Abs(distance.x) > viewPortSize.x / 2)
        {
            targetPosition.x = playerTransform.position.x - (viewPortSize.x / 2 * Mathf.Sign(distance.x));
        }

        if (Mathf.Abs(distance.y) > viewPortSize.y / 2)
        {
            targetPosition.y = playerTransform.position.y - (viewPortSize.y / 2 * Mathf.Sign(distance.y));
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition - new Vector3(0, 0, 10), ref currentVelocity, followDuration, maximumFollowSpeed);
    }

    IEnumerator CameraInit()
    {
        yield return new WaitForFixedUpdate();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        targetPosition = playerTransform.position;

        follow = true;
    }
}
