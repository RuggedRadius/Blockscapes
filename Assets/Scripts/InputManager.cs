using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : MonoBehaviour
{
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;

    public delegate void MoveTouch(Vector2 position, float time);
    public event StartTouch OnMoveTouch;

    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;

    public delegate void OrbitCamera(Finger finger);
    public event OrbitCamera OnOrbitCamera;

    public static Controls controls;

    // State
    public bool orbitting;

    // References
    private Selector selector;
    private CameraController camController;

    // Raycasting
    public LayerMask layers;
    private Ray ray;
    private RaycastHit hit;

    public List<Finger> currentActiveFingers;

    private void Awake()
    {
        controls = new Controls();
        selector = GameObject.FindGameObjectWithTag("GameController").GetComponentInChildren<Selector>();
        camController = Camera.main.GetComponent<CameraController>();
    }

    void Start()
    {
        currentActiveFingers = new List<Finger>();
    }

    private void OnEnable()
    {
        controls.Enable();
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += ctx => Touch_onFingerDown(ctx);
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += ctx => Touch_onFingerMove(ctx);
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += ctx => Touch_onFingerUp(ctx);
    }
    private void OnDisable()
    {
        controls.Disable();
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= Touch_onFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= Touch_onFingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= Touch_onFingerUp;
    }

    private void Touch_onFingerDown(Finger finger)
    {

        currentActiveFingers.Add(finger);

        // Single finger
        // Determine ray from input
        ray = Camera.main.ScreenPointToRay(new Vector3(finger.screenPosition.x, finger.screenPosition.y, 0f));

        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.green);

        if (Physics.Raycast(ray, out hit, 1000f, layers, QueryTriggerInteraction.Ignore))
        {
            // Select block
            selector.Select(hit.collider.gameObject);
        }
        else
        {
            // Orbit camera
            OnOrbitCamera?.Invoke(finger);
        }

        OnStartTouch?.Invoke(controls.Mobile.TouchPosition.ReadValue<Vector2>(), (float)finger.currentTouch.startTime);

        //Debug.Log("Finger Down");
    }

    private void Touch_onFingerMove(Finger finger)
    {
        if (currentActiveFingers.Count > 1)
        {
            // Zoom
            Debug.Log("Zooming In/Out");
        }
        else
        {
            // Single finger

            // Determine ray from input
            ray = Camera.main.ScreenPointToRay(new Vector3(finger.screenPosition.x, finger.screenPosition.y, 0f));
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.green);

            if (Physics.Raycast(ray, out hit, 1000f, layers, QueryTriggerInteraction.Ignore))
            {
                // Select block
                selector.Select(hit.collider.gameObject);
            }
            else
            {
                // Orbit camera
                //OnOrbitCamera?.Invoke(finger);
            }
        }

        OnMoveTouch?.Invoke(controls.Mobile.TouchPosition.ReadValue<Vector2>(), (float)finger.currentTouch.startTime);
        //Debug.Log("Finger Move");
    }

    private void Touch_onFingerUp(Finger finger)
    {
        currentActiveFingers.Remove(finger);
        OnEndTouch?.Invoke(controls.Mobile.TouchPosition.ReadValue<Vector2>(), (float)finger.currentTouch.startTime);
        //Debug.Log("Finger Up");
    }

    private void PinchZoom_performed(InputAction.CallbackContext obj)
    {
        if (!camController.pinchZooming)
        {
            StartCoroutine(camController.pinchZoom(obj));
        }
    }
}
