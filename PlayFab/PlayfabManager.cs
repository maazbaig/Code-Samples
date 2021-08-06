using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using PlayFab.SharedModels;

namespace Gamerjibe.Playfab
{
    public class PlayfabManager : MonoBehaviour
    {
        public static PlayfabManager Instance;
        private string Username;
        private string PlayfabID;
        private bool LoggedIn;

        public Action PlayerReady;

        private void Awake()
        {
            Instance = this;
        }

        public static void Login(string userID, string username)
        {
            PlayfabLogin.Login(userID, username);
        }

        public static void SendLeaderboard(string gameName, float score)
        {
            PlayfabLeaderboard.SendLeaderboard(gameName, score);
        }

        public static void GetLeaderboard(Action<GetLeaderboardResult> results, string gameName)
        {
            PlayfabLeaderboard.GetLeaderboard(results, gameName);
        }

        public static void UnlockItem(Item item)
        {
            PlayerInventoryManager.UnlockItem(item);
        }

        public static void GetPlayerInventory(Action<GetUserInventoryResult> results)
        {
            PlayerInventoryManager.GetInventory(results);
        }

        // GETTERS / SETTERS

        public static void SetUsername(string username)
        {
            Instance.Username = username;
        }

        public static void SetPlayfabID(string playfabID)
        {
            Instance.PlayfabID = playfabID;
        }

        public static string GetPlayfabID()
        {
            return Instance.PlayfabID;
        }

        public static string GetUsername()
        {
            return Instance.Username;
        }

        public static void SetLoggedIn(bool status)
        {
            Instance.LoggedIn = status;
        }

        public static bool IsLoggedIn()
        {
            return Instance.LoggedIn;
        }
    }
}