using System;
using System.Collections.Generic;
using UnityEngine;
using Work.JYG.Code.UI.UIContainer;
using YGPacks;

namespace Work.JYG.Code.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<string, IUI> _uiDictionary;

        private List<IUI> _btmList;

        public IUI CurrentUI { get; private set; }

        public Action OnSwipeUI;
        
        

        protected override void Awake()
        {
            base.Awake();
            _uiDictionary = new Dictionary<string, IUI>();
            _btmList = new List<IUI>();
        }

        public void AddUI(IUI ui)
        {
            if (_uiDictionary.TryGetValue(ui.Name, out IUI existingUI))
            {
                if (existingUI.GameObject.TryGetComponent<UIBase>(out UIBase uiB))
                {
                    _btmList[uiB.Index] = existingUI;
                }
                Debug.LogWarning($@"Warning! UIManager : Can't Add Ui
해당 UI : {ui.Name}이 이미 존재하는 UI입니다.
중복되는 UI Object Name : {existingUI.GameObject.name}");
                return;
            }
            _uiDictionary.Add(ui.Name, ui);
        }

        public void OpenUI(IUI ui)
        {
            if (_uiDictionary.TryGetValue(ui.Name, out IUI existingUI))
            {
                if (CurrentUI != null)
                {
                    if (CurrentUI.GameObject.TryGetComponent<ToggleUI>(out ToggleUI tgUI))
                    {
                        tgUI.CloseAnotherUI();
                    }
                    else
                    {
                        CurrentUI.CloseSelf();
                    }
                }
                CurrentUI = existingUI;
                CurrentUI.OpenSelf();
            }
            else
            {
                Debug.LogError($@"해당 되는 UI는 존재하지 않습니다.
UI Key Name : {ui.Name}
By UIManager");
            }
        }

        public void CloseCurrentUI()
        {
            CurrentUI?.CloseSelf();
        }

        public void OpenUI(IUI ui, int index)
        {
            
        }

        public bool SearchUiIsTrue(IUI ui)
        {
            if (_uiDictionary.TryGetValue(ui.Name, out IUI existingUI))
            {
                if (existingUI.GameObject.activeSelf)
                {
                    return true;
                }
                Debug.LogWarning("해당 오브젝트가 True인지 찾을 수 없습니다. Dictionary에 존재하지 않음 : UIManager" + ui.Name);
            }
            return false;
        }

        public IUI SearchUI(string name)
        {
            if (_uiDictionary.TryGetValue(name, out IUI existingUI))
            {
                return existingUI;
            }
            Debug.Log("Can't Find UI : " + name);
            return null;
        }
    }
}
