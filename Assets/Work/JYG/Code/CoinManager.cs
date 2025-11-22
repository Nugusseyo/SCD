using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGPacks;

namespace Work.JYG.Code
{
    public class CoinManager : Singleton<CoinManager>
    {
        [SerializeField] private TextMeshProUGUI coinValue;
        public int Coin { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Coin = PlayerPrefs.GetInt("Coin", 0);
            ValueChange();
        }

        public void ValueChange()
        {
            coinValue.text = $"{Coin}C";
            
            ChallengeManager.Instance.OnChallengeSwitchContacted?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PlayerPrefs.SetInt("Coin", Coin);
        }

        [ContextMenu("AddCoins")]
        public void AddDebuggingCoins()
        {
            Coin += 100000;
            ValueChange();
        }

        public void AddCoins(int value)
        {
            Coin += value;
            ValueChange();
        }
    }
}
