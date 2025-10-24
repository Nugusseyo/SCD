using System;
using UnityEngine;
using Work.JYG.Code;
using YGPacks;

public class InputManager : Singleton<InputManager>
{
    [field: SerializeField] public InputSO PlayerInput { get; private set; }

    private void Awake()
    {
        PlayerInput.OnTouch += HandleTouch;
    }

    private void OnDestroy()
    {
        PlayerInput.OnTouch -= HandleTouch;
    }

    private void HandleTouch()
    {
        Debug.Log($"터치가 감지됨 {PlayerInput.TouchPos}");
    }
}
