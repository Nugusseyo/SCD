using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Work.JYG.Code
{
    public class PieceBuySlot : MonoBehaviour
    {
        [SerializeField] private int myIndex;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private PieceUiInfo pieceUiInfo;
        [SerializeField] private Image icon;

        private string _coin = "C";
        
        StatManager statManager => StatManager.Instance;

        private void Awake()
        {
            statManager.OnPriceChanged += HandlePriceInfoReset;
            buyButton.onClick.AddListener(()=>StatManager.Instance.BuyPiece(myIndex));
            icon.sprite = pieceUiInfo.icon;
            icon.SetNativeSize();
            if (pieceUiInfo.icon != null)
            {
                myIndex = pieceUiInfo.index;
            }
        }

        private void Start()
        {
            HandlePriceInfoReset();
        }

        private void OnDestroy()
        {
            statManager.OnPriceChanged -= HandlePriceInfoReset;
        }

        private void HandlePriceInfoReset()
        {
            priceText.text = statManager.PieceStorePrice[myIndex] + _coin;
            Debug.Log("메시지 수정됨");
        }
    }
}
