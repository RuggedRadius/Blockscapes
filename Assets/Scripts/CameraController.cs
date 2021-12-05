using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class CameraController : MonoBehaviour
{
    public bool orbitting;
    public bool pinchZooming;

    public float cameraDistance;
    public float cameraHeight;

    public float rotateSpeedX;
    public float rotateSpeedY;

    public Vector3 target;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        GameController.input.OnOrbitCamera += ctx => RotateCamera(ctx);
    }

    void Update()
    {
        // Position camera
        this.transform.localPosition = new Vector3(0f, cameraHeight, -cameraDistance);

        // Look at target
        this.transform.LookAt(target);
    }

    public void RotateCamera(Finger finger)
    {
        //Debug.Log("Rotating cam");

        // Input logic of screen quarters here
        StartCoroutine(rotateCam(finger));
    }

    private IEnumerator rotateCam(Finger finger)
    {
        orbitting = true;

        float widthOfScreenQuarter = Screen.width / 4;

        Vector2 screenOffset = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 fingerPosition = finger.screenPosition - screenOffset;

        while (finger.isActive)
        {
            fingerPosition = finger.screenPosition - screenOffset;

            if (fingerPosition.x < -widthOfScreenQuarter)
            {
                // Left Hand Side
                this.transform.parent.eulerAngles += Vector3.up * Time.deltaTime * -fingerPosition.x * rotateSpeedX;
            }
            else if (fingerPosition.x > widthOfScreenQuarter)
            {
                // Right Hand Side
                this.transform.parent.eulerAngles += Vector3.up * Time.deltaTime * -fingerPosition.x * rotateSpeedX;
            }
            else
            {
                // Middle
                this.transform.parent.eulerAngles += Vector3.right * Time.deltaTime * fingerPosition.y * rotateSpeedY;
            }
            yield return null;
        }

        yield return null;
        orbitting = false;



    }

    public IEnumerator pinchZoom(InputAction.CallbackContext obj)
    {


        pinchZooming = true;

        Vector2 finger1InitialPosition = Input.GetTouch(0).position;
        //Vector2 finger1InitialPosition = obj.action.re;
        Vector2 finger2InitialPosition = Input.GetTouch(1).position;
        float initialDistance = Vector2.Distance(finger1InitialPosition, finger2InitialPosition);

        while (Input.touchCount > 1)
        {
            float currentDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            float amount = currentDistance - initialDistance;
            cameraDistance += amount * Time.deltaTime;
            yield return null;
        }

        pinchZooming = false;
        yield return null;
    }

    public void ResetNewCameraOrigin(int tierCount)
    {
        // Reset camera target to middle of new pyramid 
        this.target = (Vector3.up * (((float)tierCount) / 2f)) + (Vector3.one * 0.5f);

        // Reset camera position origin based on pyramid size
        this.cameraHeight = ((float)tierCount) / 2f;
        this.cameraDistance = (tierCount + 2) * 2.25f;
    }

    public void SetCameraTarget(Vector3 worldPosition)
    {
        this.target = worldPosition;

        this.cameraHeight = worldPosition.y * 2;
        this.cameraDistance = worldPosition.z * 2;

        this.transform.parent.position = worldPosition;
        //this.transform.localPosition = new Vector3(0, worldPosition.y, -worldPosition.z);
    }    
}
