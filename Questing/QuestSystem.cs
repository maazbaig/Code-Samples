using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    public static QuestSystem Instance;

    public List<Quest> ActiveQuests = new List<Quest>();

    private void Awake()
    {
        Instance = this;
    }

    public static void StartQuest(Quest quest)
    {
        DialogHandler.SendAlert($"Quest Started: {quest.QuestTitle}");
        Instance.ActiveQuests.Add(quest);
    }
}
