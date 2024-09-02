using System;
using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "Input/GameControl", fileName = "New Game Input")]
public class InputReaderSO : ScriptableObject, GameControls.IGamePlayActions
{
    public event Action<Vector2> moveEvent = delegate { };
    public event Action<bool> jumpStartedEvent = delegate { };
    public event Action startedRunning = delegate { };
    public event Action stoppedRunning = delegate { };
    public event Action enableMouseControlCameraEvent = delegate { };
    public event Action disableMouseControlCameraEvent = delegate { };
    public event Action<Vector2> cameraMoveEvent = delegate { };

    GameControls _controls;
    void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new GameControls();
            _controls.GamePlay.SetCallbacks(this);
        }
        
        _controls.GamePlay.Enable();
    }

    void OnDisable()
    {
        _controls.GamePlay.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpStartedEvent.Invoke(context.ReadValueAsButton());
    }

    public void OnMouseControlCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            enableMouseControlCameraEvent.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            disableMouseControlCameraEvent.Invoke();
        }
    }

    public void OnRotateCamera(InputAction.CallbackContext context)
    {
        cameraMoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                startedRunning.Invoke();
                break;
            case InputActionPhase.Canceled:
                stoppedRunning.Invoke();
                break;
        }
    }
}
