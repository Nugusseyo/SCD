using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    private const string LEADERBOARD_ID = "SCD";

    [SerializeField] private TextMeshProUGUI nameInput;
    private Ranklist ranklist;
    private bool _isReady;

    private async void Awake()
    {
        ranklist = FindAnyObjectByType<Ranklist>();
        if (ranklist == null)
        {
            Debug.LogError("Ranklist 를 찾을 수 없음");
            return;
        }

        await InitUgsAndAuth();
        _isReady = true;

        // 처음에는 Register 호출하지 않음
        // await Register();
    }

    private async Task InitUgsAndAuth()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            await UnityServices.InitializeAsync();
        }

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public void On()
    {
        _ = OnClick();
    }
    public async Task OnClick()
    {
        await SubmitAndRefresh();
    }

    private async Task SubmitAndRefresh()
    {
        if (!_isReady) return;

        try
        {
            // 이름 입력이 되어 있으면 업데이트
            if (nameInput != null && !string.IsNullOrWhiteSpace(nameInput.text))
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(nameInput.text);
            }

            // 점수 업로드
            await LeaderboardsService.Instance.AddPlayerScoreAsync(
                LEADERBOARD_ID,
                EventManager.Instance.GameTurn
            );

            // 리더보드 리스트 다시 갱신
            await Register();
        }
        catch (Exception e)
        {
            Debug.LogError($"리더보드 업로드 오류: {e}");
        }
    }

    private async Task Register()
    {
        if (!_isReady) return;

        try
        {
            var scores = await LeaderboardsService.Instance.GetScoresAsync(
                LEADERBOARD_ID,
                new GetScoresOptions
                {
                    Offset = 0,
                    Limit = ranklist.list.Count
                }
            );

            int count = Mathf.Min(ranklist.list.Count, scores.Results.Count);

            for (int i = 0; i < count; i++)
            {
                var entry = scores.Results[i];

                string displayName = entry.PlayerName;

                // 플레이어 이름이 없는 경우 대체 표시
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = $"Player {i + 1}";
                }

                // UI 적용
                ranklist.list[i].id.text   = displayName;
                ranklist.list[i].rank.text = entry.Rank.ToString();
                ranklist.list[i].turn.text = entry.Score.ToString();
                 nameInput.text = displayName;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"리더보드 오류: {e}");
        }
    }
}
