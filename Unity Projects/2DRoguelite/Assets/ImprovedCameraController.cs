using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedCameraController : MonoBehaviour
{
    [Tooltip("This is the distance at which the camera will be when the mouse is at the edge of the screen." +
             "Increase this value to make the camera move further away from the character when player moves the mouse away.")]
    [SerializeField] private float cameraDist = 3.5f;

    [Tooltip("This is the speed at which the camera will move." +
             "Increase this value to make the camera to move faster.")]
    [SerializeField] private float smoothTime = 0.2f;

    //[SerializeField] private GameObject minimapCamera;
    
    // -------------------------------
    private Transform playerTransform;
    private Camera    thisCamera;

    private Vector3 target, mousePos, refVel, shakeOffset;
    private float   zStart;

    private bool follow = false;

    private void Start()
    {
        StartCoroutine(InitCamera());

        thisCamera = GetComponent<Camera>();
        //minimapCamera.transform.parent = null;
    }

    private void FixedUpdate()
    {
        if (!follow)
            return;
        
        mousePos = CaptureMousePos();
        target = UpdateTargetPos();
        UpdateCameraPosition();

        //minimapCamera.transform.position = playerTransform.position;
    }

    private Vector3 CaptureMousePos()
    {
        Vector2 ret = thisCamera.ScreenToViewportPoint(Input.mousePosition);

        ret *= 2;
        ret -= Vector2.one;

        float max = 0.9f;

        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
            ret = ret.normalized;

        return ret;
    }

    private Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = playerTransform.position + mouseOffset;
        ret.z = zStart;

        return ret;
    }

    private void UpdateCameraPosition()
    {
        Vector3 tempPos;

        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }

    private IEnumerator InitCamera()
    {
        yield return new WaitForFixedUpdate();

        if (GameObject.FindGameObjectWithTag("Player"))
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        else
            playerTransform = gameObject.transform;


        target = playerTransform.position;
        zStart = transform.position.z;

        follow = true;
    }
}
