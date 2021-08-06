using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Quest", menuName = "Gamerjibe/Questing/Quest")]
public class Quest : ScriptableObject
{
    public string QuestTitle;
    public List<string> QuestDescription;

    public List<QuestObjective> Objectives;

    public QuestReward Reward;

    private QuestStatus Status;

    private UnityEvent AcceptQuest;
    
    public void OfferQuest()
    {
        AcceptQuest.RemoveAllListeners();
        AcceptQuest.AddListener(StartQuest);
        DialogHandler.Instance.Dialog(QuestDescription, AcceptQuest);
    }
    
    public void StartQuest()
    {
        QuestSystem.StartQuest(this);
        SetQuestStatus(QuestStatus.Started);
        InitializeObjectives();
    }


    public void CompleteObjective()
    {
        if (IsCompleted())
        {
            CompleteQuest();
        }
    }

    public bool IsCompleted()
    {
        foreach (QuestObjective objective in Objectives)
        {
            if (!objective.IsCompleted())
                return false;
        }

        return true;
    }
    
    private void InitializeObjectives()
    {
        foreach (QuestObjective questObjective in Objectives)
        {
            questObjective.SetQuest(this);
            questObjective.Reset();
        }
    }

    private void CompleteQuest()
    {
        DialogHandler.SendAlert(GetCompletedText());
        SetQuestStatus(QuestStatus.Completed);
        RewardPlayer();
    }

    public void RewardPlayer()
    {
        Reward.RewardPlayer();
    }

    public QuestStatus GetQuestStatus()
    {
        return Status;
    }

    private void SetQuestStatus(QuestStatus status)
    {
        Status = status;
    }


    public string GetCompletedText()
    {
        return $"{QuestTitle} Completed!";
    }
}

public enum QuestStatus { NotStarted, Started, Completed }
