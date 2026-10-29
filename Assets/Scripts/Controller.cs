using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Controller : MonoBehaviour
{
    private PlayerControls _playerControls;
    private float _callFrame;

    [SerializeField] private float updateInterval = 0.5f;

    public UnityEvent OnWalkForward;
    public UnityEvent OnWalkStopped;
    public UnityEvent OnWalkHoldedWithIntervals;
    public UnityEvent OnWalkHolded;
    
    public bool IsInputPressed => _playerControls.Walk.WalkForward.IsPressed();


    private void Awake()
    {
        _playerControls = new PlayerControls();

        _playerControls.Enable();
        _playerControls.Walk.WalkForward.performed += OnWalkForwardCaller;
        _playerControls.Walk.WalkForward.canceled += OnWalkStoppedCaller;
    }

    private void Update()
    {
        OnWalkHoldedCaller();
        OnWalkHoldedWithIntervalsCaller();
    }

    public void DisableControls()
    {
        _playerControls.Walk.Disable();
    }

    public void EnableControls()
    {
        _playerControls.Walk.Enable();
    }

    private void OnWalkForwardCaller(InputAction.CallbackContext context)
    {
        OnWalkForward.Invoke();
    }

    private void OnWalkHoldedCaller()
    {
        if (_playerControls.Walk.WalkForward.IsPressed())
        {
            OnWalkHolded.Invoke();
        }
    }

    private void OnWalkHoldedWithIntervalsCaller()
    {
        if (!_playerControls.Walk.WalkForward.IsPressed())
        {
            _callFrame = 0f;
            return;
        }

        _callFrame += Time.deltaTime;

        if (_callFrame < updateInterval)
        {
            return;
        }
        Debug.Log(_callFrame);
    
        OnWalkHoldedWithIntervals.Invoke();
        _callFrame = 0f;
    }

    private void OnWalkStoppedCaller(InputAction.CallbackContext context)
    {
        OnWalkStopped.Invoke();
    }
}