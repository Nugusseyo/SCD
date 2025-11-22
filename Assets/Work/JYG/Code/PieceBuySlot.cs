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

        private void Awake()
        {
            buyButton.onClick.AddListener(SpawnPiece);
            StatManager.Instance.OnPriceChanged += HandlePriceInfoReset;
            icon.sprite = pieceUiInfo.icon;
            icon.SetNativeSize();
            if (pieceUiInfo.icon != null)
            {
                myIndex = pieceUiInfo.index;
            }
        }

        private void OnDestroy()
        {
            StatManager.Instance.OnPriceChanged -= HandlePriceInfoReset;
        }

        private void SpawnPiece()
        {
            if (StatManager.Instance.PieceStorePrice[myIndex] <= CoinManager.Instance.Coin)
            {
                CoinManager.Instance.Coin -= StatManager.Instance.PieceStorePrice[myIndex];
                Debug.Log($"{StatManager.Instance.PieceStorePrice[myIndex]} 소모됨");
                CoinManager.Instance.ValueChange();
                UIManager.Instance.CurrentUI.CloseSelf();
                PieceManager.Instance.SpawnPiece(myIndex);
                StatManager.Instance.BuyPiece(myIndex);
            }
        }

        private void Start()
        {
            HandlePriceInfoReset();
        }

        private void HandlePriceInfoReset()
        {
            priceText.text = StatManager.Instance.PieceStorePrice[myIndex] + _coin;
            Debug.Log("메시지 수정됨");
        }
    }
}
