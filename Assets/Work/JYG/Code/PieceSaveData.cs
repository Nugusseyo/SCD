// SaveData.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.JYG.Code
{
    [Serializable]
    public class PieceSaveData
    {
        public Vector3Int position;
        public int currentHealth;
        public int currentEnergy;

        public int pieceIndex;          // PieceSO.pieceIndex

        public string[] attributeNames; // 붙어있는 AttributeSO 이름들
    }

    [Serializable]
    public class EnemySaveData
    {
        public string enemySOName;
        public Vector3Int position;
        public int currentHealth;
    }

    [Serializable]
    public class FullSaveData
    {
        public List<PieceSaveData> pieces = new List<PieceSaveData>();
        public List<EnemySaveData> enemies = new List<EnemySaveData>();
    }
}