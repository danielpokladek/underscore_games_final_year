using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraMidPoint;
    [SerializeField] private float     cameraRadius;
    [SerializeField] private float maximumFollowSpeed   = 10f;
    [SerializeField] private float viewPortFactor       = .5f;
    [SerializeField] private float followDuration       = .1f;

    // --------------------------
    private Camera mainCamera;
    private Vector2 viewPortSize;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private Transform playerTransform;

    private Vector2 distance;

    private bool follow = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CameraInit());

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {   
        if (!follow)
            return;
        
        // Move the mid-point of the camera, and clamp the value to the defined radius.
        // Assign the position to the cameraMidPoint object.
        Vector3 midPointPos = playerTransform.position + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position) / 2;
        //midPointPos = Vector3.ClampMagnitude(midPointPos, cameraRadius);
        //print(midPointPos);

        midPointPos = new Vector3(Mathf.Clamp(midPointPos.x, -5, 5), Mathf.Clamp(midPointPos.y, -5, 5));
        
        cameraMidPoint.position = midPointPos;
        
        
        // ---
        viewPortSize = (mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) - mainCamera.ScreenToWorldPoint(Vector2.zero)) * viewPortFactor;

        distance = cameraMidPoint.position - transform.position;
        if (Mathf.Abs(distance.x) > viewPortSize.x / 2)
        {
            targetPosition.x = cameraMidPoint.position.x - (viewPortSize.x / 2 * Mathf.Sign(distance.x));
        }

        if (Mathf.Abs(distance.y) > viewPortSize.y / 2)
        {
            targetPosition.y = cameraMidPoint.position.y - (viewPortSize.y / 2 * Mathf.Sign(distance.y));
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

    private void OnDrawGizmos()
    {
        
    }
}
