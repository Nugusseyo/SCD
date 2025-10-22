using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Work.JYG.Code.Chessboard.Pieces
{
    [CreateAssetMenu(fileName = "New VectorList", menuName = "SO/Object/Vector3Int List")]
    public class ObjectVectorListSO : ScriptableObject
    {
        [field: SerializeField] public List<Vector3Int> VectorList { get; set; }

        private void Awake()
        {
            if (VectorList == null)
            {
                VectorList = new List<Vector3Int>();
            }
        }
    }
}
