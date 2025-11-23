using System;
using System.Collections;
using csiimnida.CSILib.SoundManager.RunTime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YGPacks;

namespace Work.JYG.Code
{
    public class ChallengeManager : Singleton<ChallengeManager>
    {
        private RectTransform _myObj;
        [SerializeField] private RectTransform height;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI details;

        [SerializeField] private Image icon;

        [SerializeField] private Sprite[] icons;

        public Action OnChallengeSwitchContacted;

        protected override void Awake()
        {
            base.Awake();
            _myObj = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            ResetMyPos();
            OnChallengeSwitchContacted += HandleChallengeDetector;
        }

        private void HandleChallengeDetector()
        {
            if (PlayerPrefs.GetInt("BossDie") == 1 && PlayerPrefs.GetInt("C0") != 1)
            {
                DownMyPos("[첫 트로피]", "처음으로 보스를 죽이세요.", 0);
                PlayerPrefs.SetInt("C0", 1);
            }
            else if ( PlayerPrefs.GetInt("EnemyDie") == 25 && PlayerPrefs.GetInt("C1") != 1)
            {
                DownMyPos("[체스 전략가]", "적을 25번 처치하세요.", 0);
                PlayerPrefs.SetInt("C1", 1);
            }
            else if (PlayerPrefs.GetInt("UpgradeNum") == 10&& PlayerPrefs.GetInt("C2") != 1)
            {
                DownMyPos("[대장장이]", "기물을 10번 강화하세요.", 0);
                PlayerPrefs.SetInt("C2", 1);
            }
            else if (PlayerPrefs.GetInt("PieceDie") == 1 && PlayerPrefs.GetInt("C3") != 1)
            {
                DownMyPos("[어이쿠, 실수]", "1개의 기물이 죽었습니다.", 0);
                PlayerPrefs.SetInt("C3", 1);
            }
            else if (PlayerPrefs.GetInt("NameChange") == 1 && PlayerPrefs.GetInt("C4") != 1)
            {
                DownMyPos("[명함]", "자신의 이름을 리더보드에 등록하세요.", 0);
                PlayerPrefs.SetInt("C4", 1);
            }
            else if (PlayerPrefs.GetInt("SpawnPiece") == 20 && PlayerPrefs.GetInt("C5") != 1)
            {
                DownMyPos("[나는 군단이다]", "기물을 20번 소환하세요.", 0);
                PlayerPrefs.SetInt("C5", 1);
            }
            else if (PlayerPrefs.GetInt("GameOver") == 1 && PlayerPrefs.GetInt("C6") != 1)
            {
                DownMyPos("[회귀]", "1회 패배하세요.", 0);
                PlayerPrefs.SetInt("C6", 1);
            }
            else if (PlayerPrefs.GetInt("Coin") >= 1500 && PlayerPrefs.GetInt("C7") != 1)
            {
                DownMyPos("[갑부]", "1500원을 소지하고 있습니다.", 0);
                PlayerPrefs.SetInt("C7", 1);
            }
            else if (PlayerPrefs.GetInt("GameTurn") > 60 && PlayerPrefs.GetInt("C8") != 1)
            {
                DownMyPos("[이 게임 정말 재밌어요]", "60스테이지를 돌파하세요.", 0);
                PlayerPrefs.SetInt("C8", 1);
            }
            else if (PlayerPrefs.GetInt("BossDie") == 4 && PlayerPrefs.GetInt("C9") != 1)
            {
                DownMyPos("[세계의 영웅]", "보스를 4번 쓰러트리세요.", 0);
                PlayerPrefs.SetInt("C9", 1);
            }
            else if (PlayerPrefs.GetInt("End") == 1 && PlayerPrefs.GetInt("C10") != 1)
            {
                DownMyPos("[끝]", "모든 보스를 처치하세요.", 0);
                PlayerPrefs.SetInt("C10", 1);
            }
            AttributeUiManager.Instance.CanAttributeOn();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnChallengeSwitchContacted -= HandleChallengeDetector;
        }


        [ContextMenu("ResetPos")]
        public void ResetMyPos()
        {
            _myObj.localPosition = new Vector2(0, height.sizeDelta.y);
        }

        [ContextMenu("ShowTitle")]
        public void ShowTile()
        {
            DownMyPos("테스트용 제목입니다!", "이건 테스트용 설명입니다. 2턴 후에 발동된다네요.", 0);
        }
        public void DownMyPos(string title, string details, int iconIndex)
        {
            this.title.text = title;
            this.details.text = details;
            this.icon.sprite = icons[iconIndex];
            _myObj.DOAnchorPos(new Vector2(0, 0), 0.8f).SetEase(Ease.InOutQuart);
            StartCoroutine(DisShowTitle());
            SoundManager.Instance.PlaySound("Ringing");
        }

        private IEnumerator DisShowTitle()
        {
            yield return new WaitForSeconds(5f);
            UpMyPos();
        }

        [ContextMenu("DisableTitle")]
        public void UpMyPos()
        {
            _myObj.DOAnchorPos(new Vector2(0, height.sizeDelta.y), 0.8f).SetEase(Ease.InQuart).OnComplete(() => OnChallengeSwitchContacted?.Invoke());
        }
    }
}
