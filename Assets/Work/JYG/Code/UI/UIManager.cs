using System.Collections.Generic;
using UnityEngine;
using YGPacks;

namespace Work.JYG.Code.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<string, IUI> _uiDictionary;

        public IUI CurrentUI;

        protected override void Awake()
        {
            base.Awake();
            _uiDictionary = new Dictionary<string, IUI>();
        }

        public void AddUI(IUI ui)
        {
            if (_uiDictionary.TryGetValue(ui.Name, out IUI existingUI))
            {
                Debug.LogWarning($@"Warning! UIManager : Can't Add Ui
해당 UI : {ui.Name}이 이미 존재하는 UI입니다.
중복되는 UI Object Name : {existingUI.GameObject.name}");
                return;
            }
            _uiDictionary.Add(ui.Name, ui);
        }

        public void OpenUI(string uiName)
        {
            if (_uiDictionary.TryGetValue(uiName, out IUI existingUI))
            {
                if (CurrentUI != null)
                {
                    CurrentUI.CloseSelf();
                }
                CurrentUI = existingUI;
                CurrentUI.OpenSelf();
            }
            else
            {
                Debug.LogError($@"해당 되는 UI는 존재하지 않습니다.
UI Key Name : {uiName}
By UIManager");
                return;
            }
        }

        public IUI SearchCurrentUI()
        {
            if (CurrentUI != null)
            {
                return CurrentUI;
            }
            Debug.Log("Current UI Is Null");
            return null;
        }
    }
}
