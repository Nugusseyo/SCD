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
    private TextMeshProUGUI _text;
    string leaderboardIndex;
    [SerializeField] private double example;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public void OnClick()
    {
        _ = register();
    }
        private async Task register()
    {
        Debug.Log("헤헤: register 시작");

        try
        {
            string playerName = _text.text;

            // 점수 등록
            var entry = await LeaderboardsService.Instance.AddPlayerScoreAsync("SCD", example);
            Debug.Log($"AddPlayerScore 완료: Rank={entry.Rank}, Score={entry.Score}");

            // 점수 가져오기
            var fetched = await LeaderboardsService.Instance.GetPlayerScoreAsync("SCD");
            Debug.Log($"GetPlayerScore: Rank={fetched.Rank}, Score={fetched.Score}, Name={fetched.PlayerName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"리더보드 오류: {e}");
        }
    }

}
