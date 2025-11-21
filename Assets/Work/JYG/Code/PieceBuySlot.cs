using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.JYG.Code.UI;
using Work.PTY.Scripts.PieceManager;

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

        private void OnEnable()
        {
            StatManager.Instance.OnPriceChanged += HandlePriceInfoReset;
            buyButton.onClick.AddListener(()=>StatManager.Instance.BuyPiece(myIndex));
            buyButton.onClick.AddListener(SpawnPiece);
            icon.sprite = pieceUiInfo.icon;
            icon.SetNativeSize();
            if (pieceUiInfo.icon != null)
            {
                myIndex = pieceUiInfo.index;
            }
        }

        private void SpawnPiece()
        {
            UIManager.Instance.CurrentUI.CloseSelf();
            PieceManager.Instance.SpawnPiece(myIndex);
        }

        private void Start()
        {
            HandlePriceInfoReset();
        }

        private void OnDisable()
        {
            StatManager.Instance.OnPriceChanged -= HandlePriceInfoReset;
        }

        private void HandlePriceInfoReset()
        {
            priceText.text = StatManager.Instance.PieceStorePrice[myIndex] + _coin;
            Debug.Log("메시지 수정됨");
        }
    }
}
