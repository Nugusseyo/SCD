using System;
using UnityEngine;
using YGPacks;

namespace Work.JYG.Code
{
    [DefaultExecutionOrder(-10)]
    public class StatManager : Singleton<StatManager>
    {
        public PieceListSO pieceList;
        public string[] InfoStrings { get; private set; } =
            { "Pawn", "Knight", "Bishop", "Rook", "Queen", "King", "Damage", "Health", "Price", "PUL", "PiecePrice" };
        public int[] PieceDamage { get; private set; } = new int[6];
        public int[] PieceHealth { get; private set; } = new int[6];
        public int[] PieceStorePrice { get; private set; } = new int[6];
        public int[] PieceUpgradePrice { get; private set; } = new int[6];
        public int[] PieceUpgradeLevel { get; private set; } = new int[6];
        public int[] ReturnPieceHealth { get; set; } = new int[6];
        public int[] ReturnPieceDamage { get; set; } = new int[6];

        private const int CHESS_PIECE_COUNT = 6;
        
        public Action OnPriceChanged;

        protected override void Awake()
        {
            base.Awake();
            
            LoadMyValue();
        }
        [ContextMenu("LoadMyValue")]
        private void LoadMyValue()
        {
            for (int i = 0; i < CHESS_PIECE_COUNT; i++)
            {
                PieceDamage[i] = PlayerPrefs.GetInt(InfoStrings[i] + InfoStrings[6]);
                PieceHealth[i] = PlayerPrefs.GetInt(InfoStrings[i] + InfoStrings[7]);
                PieceUpgradePrice[i] = PlayerPrefs.GetInt(InfoStrings[i] + InfoStrings[8]);
                PieceUpgradeLevel[i] = PlayerPrefs.GetInt(InfoStrings[i] + InfoStrings[9]);
                PieceStorePrice[i] = PlayerPrefs.GetInt(InfoStrings[i] + InfoStrings[10]);
            }

            if (PieceDamage[0] == 0)
            {
                for (int i = 0; i < CHESS_PIECE_COUNT; i++)
                {
                    PieceDamage[i] = pieceList.pieces[i].damage;
                    PieceHealth[i] = pieceList.pieces[i].health;
                    UpgradeMyLevel(i);
                    BuyPiece(i);
                }
            }

            EventManager.Instance.OnTurnChanged?.Invoke();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                for (int i = 0; i < CHESS_PIECE_COUNT; i++)
                {
                    PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[6], PieceDamage[i]);
                    PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[7], PieceHealth[i]);
                    PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[8], PieceUpgradePrice[i]);
                    PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[9], PieceUpgradeLevel[i]);
                    PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[10], PieceStorePrice[i]);
                }
                
            }
        }

        private void OnApplicationQuit()
        {
            for (int i = 0; i < CHESS_PIECE_COUNT; i++)
            {
                PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[6], PieceDamage[i]);
                PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[7], PieceHealth[i]);
                PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[8], PieceUpgradePrice[i]);
                PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[9], PieceUpgradeLevel[i]);
                PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[10], PieceStorePrice[i]);
            }
        }
        public void UpgradeMyLevel(int pieceIndex)
        {
            PieceUpgradePrice[pieceIndex] += Mathf.RoundToInt(25 * (pieceIndex + 1));
            PieceUpgradeLevel[pieceIndex] += 1;
            PieceHealth[pieceIndex] += pieceList.pieces[pieceIndex].healthIncAmt;
            PieceDamage[pieceIndex] += pieceList.pieces[pieceIndex].damageIncAmt;
            OnPriceChanged?.Invoke();
        }

        public void BuyPiece(int pieceIndex)
        {
            int newValue = PieceStorePrice[pieceIndex] + Mathf.RoundToInt((150 * (((float)(pieceIndex* pieceIndex) + 1 ) / 6)));
            if (newValue < (pieceIndex + 1) * 250 + ((pieceIndex + 1)* 25))
            {
                PieceStorePrice[pieceIndex] = newValue;
                OnPriceChanged?.Invoke();
            }
        }

        [ContextMenu("Value Changed")]
        public void InvokePriceChanged()
        {
            OnPriceChanged?.Invoke();
        }

        [ContextMenu("Reset")]
        public void ResetAllRegister()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
