using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnTxt;
    [SerializeField] private Image icon;

    public List<Sprite> sprites = new List<Sprite>();
    void Start()
    {
        EventManager.Instance.OnTurnChanged += HandleReloadTxt;
        HandleReloadTxt();
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnTurnChanged -= HandleReloadTxt;
    }

    private void HandleReloadTxt()
    {
        if (EventManager.Instance.GameTurn % 30 == 25)
        {
            icon.sprite = sprites[1];
        }
        else if (EventManager.Instance.GameTurn % 30 == 29)
        {
            icon.sprite = sprites[2];
        }
        else
        {
            icon.sprite = sprites[0];
        }
        turnTxt.text = EventManager.Instance.GameTurn.ToString();
    }
}
