using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamerjibe.Playfab
{
    public class PlayfabLeaderboard
    {
        public static void SendLeaderboard(string gameName, float score)
        {
            score *= 100;

            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = gameName,
                    Value = (int)score
                }
            }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, PlayfabLeaderboardCallbacks.OnLeaderboardUpdate, PlayfabCallbacks.LogFailure);
        }


        public static void GetLeaderboard(Action<GetLeaderboardResult> results, string gameName)
        {
            var request = new GetLeaderboardRequest
            {
                StatisticName = gameName,
                StartPosition = 0,
                MaxResultsCount = 10
            };

            PlayFabClientAPI.GetLeaderboard(request, results, PlayfabCallbacks.LogFailure);
        }
    }
}
