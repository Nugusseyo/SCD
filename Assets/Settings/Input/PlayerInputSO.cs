using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "Scriptable Objects/PlayerInputSO")]
public class PlayerInputSO : ScriptableObject, IPlayerActions
{
    private Controls _controls;

    public Action OnPlayerTouched;
    public Action OnPlayerTouchEnded;
    public Action OnUiTouched;
    public Action OnUiTouchEnded;
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
        }
        
        _controls.Player.Enable();
        _controls.Player.SetCallbacks(this);
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
        _controls.UI.Disable();
    }

    public void TurnMyInput(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("My Input is " + isOn);
            _controls.Player.Enable();
        }
        else
        {
            Debug.Log("My Input is " + isOn);
            _controls.Player.Disable();
        }
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPlayerTouched?.Invoke();
        }

        if (context.canceled)
        {
            OnPlayerTouchEnded?.Invoke();
        }
    }
}
