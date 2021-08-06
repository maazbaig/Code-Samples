using Gamerjibe.Tabs;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.SharedModels;
using System.Collections.Generic;
using UnityEngine;

namespace Gamerjibe.Playfab
{
    public static class PlayfabCallbacks
    {
        public static void LogSuccess(PlayFabResultCommon result)
        {
            var requestName = result.Request.GetType().Name;
            GJLogger.Debug("[Playfab] " + requestName + " successful");
        }

        public static void LogFailure(PlayFabError error)
        {
            GJLogger.Debug("[Playfab] " + error.GenerateErrorReport());
        }
    }

    public static class PlayfabInventoryCallbacks
    {
        public static void GotInventory(GetUserInventoryResult result)
        {
            PlayerInventoryManager.AddItemsToPlayerCustomizer(result.Inventory);
        }

        public static void FailedToUnlock(ItemUnlockError error)
        {
            GJLogger.Debug($"Playfab: Failed to Unlock Item. {error.ToString()}");
            if(error == ItemUnlockError.Duplicate)
            {
                DialogHandler.SendNotification(ItemUnlocker.GetAlreadyUnlockedString());
                TabMenu.SetLoadingStatus(false, false);
            }
        }
    }

    public enum ItemUnlockError { Duplicate };

    public static class PlayfabLeaderboardCallbacks
    {
        public static void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
        {
            GJLogger.Debug("[PlayFab] Successfully sent leaderboard");
        }
    }

    public static class PlayfabLoginCallbacks
    {
        public static void OnSuccessLogin(LoginResult result)
        {
            if (PlayfabManager.IsLoggedIn()) return;

            GJLogger.Debug($"[PlayFab] Successfully Logged in");
            if (result.NewlyCreated)
                PlayfabLogin.SetUsername(PlayfabManager.GetUsername());

            PlayfabManager.SetPlayfabID(result.PlayFabId);
            PlayfabManager.SetLoggedIn(true);
            PlayfabManager.Instance.PlayerReady.Invoke();
        }

    }
}
