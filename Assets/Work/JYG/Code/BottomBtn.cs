using System;
using UnityEngine;
using UnityEngine.UI;
using Work.JYG.Code.UI;
using Work.JYG.Code.UI.UIContainer;

namespace Work.JYG.Code
{
    [DefaultExecutionOrder(1)]
    public class BottomBtn : UIBase, IUI, ToggleUI
    {
        private Image _myImage;
        private Image _selfImg;

        [SerializeField]
        private int myIndex;
    
        public bool IsActive { get; private set; } = false;
        [SerializeField] private BtSwapSO btnData;
        public override string Name => gameObject.name;
        public override GameObject GameObject => gameObject;

        private void Awake()
        {
            _myImage = transform.GetChild(0).GetComponentInChildren<Image>();
            _selfImg = GetComponent<Image>();
            _myImage.sprite = btnData.falseImg;
            _selfImg.sprite = btnData.fBgImg;
            Index = myIndex;
            UIManager.Instance.OnSwipeUI += ChangeCurrentBtn;
        }

        protected override void Start()
        {
            base.Start();
        }

        private void SwapBtnSprite(bool active)
        {
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
            IsActive = true;
            SwapBtnSprite(true);
            if (SwipeManager.Instance.IsActive)
            {
                SwipeManager.Instance.SwipeUI.SetCurrentPage(Index);
            }
            else
            {
                SwipeManager.Instance.OpenSelf();
                SwipeManager.Instance.SwipeUI.SetCurrentPage(Index);
            }
        }

        public override void CloseSelf()
        {
            IsActive = false;
            SwapBtnSprite(false);
            SwipeManager.Instance.CloseSelf();
        }

        public void BtnClick()
        {
            if (IsActive)
            {
                UIManager.Instance.CloseCurrentUI();
                return;
            }
            UIManager.Instance.OpenUI(this);
        }

        public void CloseAnotherUI()
        {
            IsActive = false;
            SwapBtnSprite(false);
        }

        private void ChangeCurrentBtn()
        {
            if (SwipeManager.Instance.SwipeUI.CurrentPage == Index)
            {
                BtnClick();
            }
            else
            {
                CloseAnotherUI();
            }
        }
    }
}
