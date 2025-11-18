using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.JYG.Code;

public class StoreElement : MonoBehaviour
{
    [SerializeField] private int myIndex;
    
    [SerializeField] private TextMeshProUGUI myValue;
    [SerializeField] private TextMeshProUGUI myPrice;

    private void Awake()
    {
        UpdateMyValue();
    }

    public void BuyPiece()
    {
        StatManager.Instance.UpgradeMyLevel(myIndex);
        UpdateMyValue();
    }

    private void UpdateMyValue()
    {
        myPrice.text = $"{StatManager.Instance.PieceUpgradePrice[myIndex]}";
        myValue.text =
            $"Damage : {StatManager.Instance.PieceDamage[myIndex]} -> {StatManager.Instance.PieceDamage[myIndex] + ((myIndex + 1) * 2)}" +
            $", Hp : {StatManager.Instance.PieceHealth[myIndex]} -> {StatManager.Instance.PieceHealth[myIndex] + ((myIndex + 1) * 20)}";
    }
}
