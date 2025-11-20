using System;
using UnityEngine;

namespace Work.JYG.Code.Debugging
{
    public class PieceDebugger : MonoBehaviour
    {
        [SerializeField] private Piece piece;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log($@"The Piece Name : {piece.Name}
Piece's MaxHealth = {piece.MaxHealth}
Piece's Current Health = {piece.CurrentHealth}

Attack is : {piece.AttackDamage}
MaxEnergy = {piece.MaxEnergy}
CurrentEnergy = {piece.CurrentEnergy}
PieceType is [{piece.pieceData.type}]");
            }
        }

        public void PrintDebuggingMessage()
        {
            Debug.Log("Debug Message!");
        }
    }
}
