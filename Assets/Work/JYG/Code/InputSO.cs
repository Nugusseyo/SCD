using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Work.JYG.Code
{
    [CreateAssetMenu(fileName = "New Input", menuName = "SO/Input", order = 15)]
    public class InputSO : ScriptableObject, TestInput.IPlayerActions
    {
        public TestInput TestInput { get; private set; }

        public Action OnTouch;
        public Vector2 TouchPos { get; private set; }

        public int value;
        
        private void OnEnable()
        {
            if (TestInput == null)
            {
                TestInput = new TestInput();
                TestInput.Player.SetCallbacks(this);
            }
            TestInput.Player.Enable();
        }

        private void OnDisable()
        {
            TestInput.Player.Disable();
        }


        public void OnNewaction(InputAction.CallbackContext context)
        {
            TouchPos = context.ReadValue<Vector2>();
            OnTouch?.Invoke();
            // Event -> Action, UnityEvent
            // Action -> void, UnityEvent -> void X
            // Action -> 구독, 구독 해제를 통해 Action에 저장
            // 예) 총알이 나간다. 소리가 난다. 화면이 흔들린다. -> Weapon.Shot(); AudioManager.Play(); EffectManager.Shake();
            // 예) 걷고, 뛰고, 구르고, 스킬쓰고 
            // Action += Weapon.Shot(); AudioManager.Play(); EffectManager.Shake();
        }
    }
}