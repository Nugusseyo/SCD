using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using YGPacks;

namespace Work.JYG.Code.UI
{
    public class SwipeManager : Singleton<SwipeManager>, IUI
    {
        private LayoutGroup _layoutGroup;
        public SwipeUI SwipeUI { get; private set; }
        private List<RectTransform> _childRectTransform;
        private float _downPosY;

        [SerializeField] private GameObject slot;

        public bool IsActive { get; private set; } = false;
    
        [SerializeField] private RectTransform btnParent;

        protected override void Awake()
        {
            _childRectTransform = new List<RectTransform>();
            SwipeUI = GetComponentInChildren<SwipeUI>();
            _layoutGroup = GetComponentInChildren<LayoutGroup>();
        
            _childRectTransform = SwipeUI.GetComponentsInChildren<RectTransform>().ToList();
            _childRectTransform.RemoveAt(0);
            StartCoroutine(GetInfoAfterLayout());
            
            UIManager.Instance.AddUI(this);
        }


        public string Name => "SwipeManager";
        public GameObject GameObject => gameObject;
        [ContextMenu("Swipe")]
        public void OpenSelf()
        {
            IsActive = true;
            btnParent.DOKill();
            /*
            _layoutGroup.enabled = false;
            StartCoroutine(WaitOneFrame(() =>
                btnParent.DOAnchorPosY(0, 1f).SetEase(Ease.OutQuart).OnComplete(() 
                    => _layoutGroup.enabled = true)));*/
            StopAllCoroutines();
            slot.SetActive(true);
            StartCoroutine(MoveToPos(0, false));

        }

        private IEnumerator MoveToPos(float pos, bool isShut)
        {
            while (Mathf.Abs(btnParent.anchoredPosition.y - pos) > 0.1f)
            {
                yield return null;
                Vector2 anchoredPosition = btnParent.anchoredPosition;
                anchoredPosition.y = Mathf.MoveTowards(anchoredPosition.y, pos, 5000 * Time.deltaTime);
                btnParent.anchoredPosition = anchoredPosition;
            }

            if (isShut)
            {
                slot.SetActive(false);
            }
        }

        [ContextMenu("Down")]
        public void CloseSelf()
        {
            
            IsActive = false;/*
            btnParent.DOKill();
            _layoutGroup.enabled = false;
            StartCoroutine(WaitOneFrame(() =>
                btnParent.DOAnchorPosY(_downPosY, 1f).SetEase(Ease.OutQuart).OnComplete(()
                    => _layoutGroup.enabled = true)));
                    */
            StopAllCoroutines();
            StartCoroutine(MoveToPos(_downPosY, true));
        }

        private IEnumerator GetInfoAfterLayout()
        {
            yield return new WaitForEndOfFrame();
            _downPosY = _childRectTransform[0].rect.height * -1;

            IsActive = false;
            btnParent.DOKill();
            _layoutGroup.enabled = false;
            StartCoroutine(WaitOneFrame(() =>
                btnParent.DOAnchorPosY(_downPosY, 0f).SetEase(Ease.OutQuart).OnComplete(()
                    => _layoutGroup.enabled = true)));
            slot.SetActive(false);
        }

        private IEnumerator WaitOneFrame(Action action)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(btnParent);
            yield return null;
            action?.Invoke();
        }

    }
}
