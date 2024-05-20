using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "UserInput", fileName = "New Input")]
public class InputReader : ScriptableObject,InputSettings.IGamePlayActions
{
    InputSettings _inputSettings;
    Vector2 _moveInput;
    bool _jumpInput;
    Vector2 _cameraInput;
    public Vector2 MoveInput => _moveInput;
    public bool JumpInput => _jumpInput;
    public Vector2 CameraInput => _cameraInput;
    void OnEnable()
    {
        _inputSettings ??= new InputSettings();
        _inputSettings.GamePlay.AddCallbacks(this);
        _inputSettings.GamePlay.Enable();
    }

    void OnDisable()
    {
        _inputSettings.GamePlay.Disable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _jumpInput = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _jumpInput = false;
        }
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        _cameraInput = context.ReadValue<Vector2>();
    }
}
