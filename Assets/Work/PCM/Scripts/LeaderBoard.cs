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
    [SerializeField] private double example;
    [SerializeField] private string _inputField;
    private bool islogin = false;

    private void Awake()
    {
       
       _ =AwakeSet();
    }

    private async Task AwakeSet()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await register();
    }    
    private async Task register()
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(_inputField);

            var entry = await LeaderboardsService.Instance.AddPlayerScoreAsync("SCD", example);

            var fetched = await LeaderboardsService.Instance.GetPlayerScoreAsync("SCD");
            Debug.Log(fetched.PlayerName.ToString().Substring(0,fetched.PlayerName.ToString().Length-5));

        }
        catch (Exception e)
        {
            Debug.LogError($"리더보드 오류: {e}");
        }
    }

}
