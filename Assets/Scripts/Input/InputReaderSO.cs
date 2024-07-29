using System;
using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "Input/GameControl", fileName = "New Game Input")]
public class InputReaderSO : ScriptableObject, GameControls.IGamePlayActions
{
    public event Action<Vector2> OnMoveEvent = delegate { };
    public event Action<bool> OnJumpStartEvent = delegate { };

    GameControls _controls;
    void OnEnable()
    {
        _controls ??= new GameControls();
        _controls.GamePlay.SetCallbacks(this);
        _controls.GamePlay.Enable();
    }

    void OnDisable()
    {
        _controls?.GamePlay.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        OnJumpStartEvent.Invoke(context.ReadValueAsButton());
    }
}
