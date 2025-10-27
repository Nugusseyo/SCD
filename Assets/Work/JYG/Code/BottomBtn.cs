using System;
using UnityEngine;
using UnityEngine.UI;
using Work.JYG.Code.UI;
using Work.JYG.Code.UI.UIContainer;

public class BottomBtn : UIBase, IUI
{
    private Image _myImage;
    private Image _selfImg;

    [SerializeField]
    private int myIndex;
    
    private bool _isActive = false;
    [SerializeField] private BtSwapSO btnData;
    public override string Name => gameObject.name;
    public override GameObject GameObject => gameObject;

    protected override void Awake()
    {
        base.Awake();
        _myImage = transform.GetChild(0).GetComponentInChildren<Image>();
        _selfImg = GetComponent<Image>();
        _myImage.sprite = btnData.falseImg;
        _selfImg.sprite = btnData.fBgImg;
        Index = myIndex;
    }

    private void SwapBtnSprite(bool active)
    {
        Debug.Log("SwapBtnSprite");
        if (active)
        {
            _myImage.sprite = btnData.trueImg;
            _selfImg.sprite = btnData.tBgImg;
        }
        else
        {
            _myImage.sprite = btnData.falseImg;
            _selfImg.sprite = btnData.fBgImg;
        }
    }

    public override void OpenSelf()
    {
        _isActive = true;
        SwapBtnSprite(true);
    }

    public override void CloseSelf()
    {
        _isActive = false;
        SwapBtnSprite(false);
    }

    public void BtnClick()
    {
        if (_isActive)
        {
            Work.JYG.Code.UI.UIManager.Instance.CloseCurrentUI();
            return;
        }
        Work.JYG.Code.UI.UIManager.Instance.OpenUI(this);
    }
}
