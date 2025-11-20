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
    [SerializeField] private TMP_InputField _inputField;
    private bool islogin = false;

    private void Awake()
    {
       
       _ =AwakeSet();
    }

    private async Task AwakeSet()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        islogin = true;
    }

    private void Update()
    {
        if (islogin)
        {
            _ = register();
            islogin = false;
        }
    }
    

    private async Task register()
    {
        try
        {
            var entry = await LeaderboardsService.Instance.AddPlayerScoreAsync("SCD", example);
            Debug.Log($"AddPlayerScore 완료: Rank={entry.Rank}, Score={entry.Score}");

            var fetched = await LeaderboardsService.Instance.GetPlayerScoreAsync("SCD");
            Debug.Log($"GetPlayerScore: Rank={fetched.Rank}, Score={fetched.Score}, Name={fetched.PlayerName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"리더보드 오류: {e}");
        }
    }

}
