using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamerjibe.Playfab
{
    public class PlayfabLogin
    {
        public static void Login(string userID, string username)
        {
            var request = new LoginWithCustomIDRequest
            {
                CustomId = userID,
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                },
            };

            PlayfabManager.SetUsername(username);

            PlayFabClientAPI.LoginWithCustomID(request, PlayfabLoginCallbacks.OnSuccessLogin, PlayfabCallbacks.LogFailure);
        }


        public static void SetUsername(string username)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = username
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, PlayfabCallbacks.LogSuccess, PlayfabCallbacks.LogFailure);
        }

    }
}
