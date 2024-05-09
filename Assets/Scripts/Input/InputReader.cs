using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour,InputSettings.IGamePlayActions
{
    InputSettings _inputSettings;
    Vector2 _moveInput;
    bool _jumpInput;

    public Vector2 MoveInput => _moveInput;
    public bool JumpInput => _jumpInput;
    void Awake()
    {
        _inputSettings = new InputSettings();
        _inputSettings.GamePlay.AddCallbacks(this);
        _inputSettings.GamePlay.Enable();
    }

    void OnEnable()
    {
        _inputSettings?.Enable();
    }

    void OnDisable()
    {
        _inputSettings?.Disable();
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
}
