using System;
using TMPro;
using UnityEngine;
using Work.JYG.Code;

public class StoreElement : MonoBehaviour
{
    [SerializeField] private int myIndex;
    
    [SerializeField] private TextMeshProUGUI myValue;

    private void Awake()
    {
        UpdateMyValue();
    }

    private void UpdateMyValue()
    {
        myValue.text =
            $"Damage : {StatManager.Instance.PieceDamage[myIndex]} -> {StatManager.Instance.PieceDamage[myIndex] + ((myIndex + 1) * 2)}" +
            $", Hp : {StatManager.Instance.PieceHealth[myIndex]} -> {StatManager.Instance.PieceHealth[myIndex] + ((myIndex + 1) * 20)}";
    }
}
