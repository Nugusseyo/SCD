using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Work.JYG.Code.UI;

public class SwipeManager : MonoBehaviour, IUI
{
    private SwipeUI _swipeUI;
    private List<RectTransform> _childRectTransform;
    private float _upPosY;
    private float _downPosY;
    
    [SerializeField] private GameObject btnParent;

    private void Awake()
    {
        _childRectTransform = new List<RectTransform>();
        _swipeUI = GetComponentInChildren<SwipeUI>();
        
        _childRectTransform = _swipeUI.GetComponentsInChildren<RectTransform>().ToList();
        _childRectTransform.RemoveAt(0);
        
        _upPosY = _childRectTransform[0].anchoredPosition.y;
        _downPosY = _childRectTransform[0].sizeDelta.y * -1;
    }


    public string Name => "SwipeManager";
    public GameObject GameObject => gameObject;
    [ContextMenu("Swipe")]
    public void OpenSelf()
    {
        foreach (RectTransform child in _childRectTransform)
        {
            child.DOAnchorPosY(_upPosY, 0.3f).SetEase(Ease.OutBack);
        }
    }
    [ContextMenu("Down")]
    public void CloseSelf()
    {
        foreach (RectTransform child in _childRectTransform)
        {
            child.DOAnchorPosY(_downPosY, 0.3f).SetEase(Ease.OutBack);
        }
    }
}
