using System;
using UnityEngine;
using YGPacks;

namespace Work.JYG.Code
{
    [DefaultExecutionOrder(-10)]
    public class StatManager : Singleton<StatManager>
    {
        public string[] InfoStrings { get; private set; } =
            { "Pawn", "Knight", "Bishop", "Rook", "Queen", "King", "Damage", "Health", "Price", "PUL" };
        public int[] PieceDamage { get; private set; } = new int[6];
        public int[] PieceHealth { get; private set; } = new int[6];
        public int[] PiecePrice { get; private set; } = new int[6];
        public int[] PieceUpgradeLevel { get; private set; } = new int[6];

        private const int CHESS_PIECE_COUNT = 6;

        protected override void Awake()
        {
            base.Awake();
            
            LoadMyValue();
        }

        private void LoadMyValue()
        {
            for (int i = 0; i < CHESS_PIECE_COUNT; i++)
            {
                PieceDamage[i] = PlayerPrefs.GetInt(InfoStrings[i] + InfoStrings[6]);
                PieceHealth[i] = PlayerPrefs.GetInt(InfoStrings[i] + InfoStrings[7]);
                PiecePrice[i] = PlayerPrefs.GetInt(InfoStrings[i] + InfoStrings[8]);
                PieceUpgradeLevel[i] = PlayerPrefs.GetInt(InfoStrings[i] + InfoStrings[9]);
            }

            if (PieceDamage[0] == 0)
            {
                for (int i = 0; i < CHESS_PIECE_COUNT; i++)
                {
                    UpgradeMyLevel(i);
                }
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                for (int i = 0; i < CHESS_PIECE_COUNT; i++)
                {
                    PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[6], PieceDamage[i]);
                    PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[7], PieceHealth[i]);
                    PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[8], PiecePrice[i]);
                    PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[9], PieceUpgradeLevel[i]);
                }
                
            }
        }

        private void OnApplicationQuit()
        {
            for (int i = 0; i < CHESS_PIECE_COUNT; i++)
            {
                PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[6], PieceDamage[i]);
                PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[7], PieceHealth[i]);
                PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[8], PiecePrice[i]);
                PlayerPrefs.SetInt(InfoStrings[i] + InfoStrings[9], PieceUpgradeLevel[i]);
            }
        }

        public void UpgradeMyLevel(int pieceIndex)
        {
            PiecePrice[pieceIndex] += 15 * (int)(pieceIndex / 3);
            PieceUpgradeLevel[pieceIndex] += 1;
            PieceHealth[pieceIndex] += (pieceIndex + 1) * 20;
            PieceDamage[pieceIndex] += (pieceIndex + 1) * 2;
        }
    }
}
