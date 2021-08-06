using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisitChatAreas", menuName = "Gamerjibe/Questing/QuestObjectives/VisitChatAreas")]
public class QuestObjective_VisitChatAreas : QuestObjective
{
    public int NeedToVisitAmount = 2;
    public int VisitedAmount = 0;

    protected override void IncrementProgress()
    {
        VisitedAmount++;
        DialogHandler.SendAlert($"Visited {VisitedAmount} out of {NeedToVisitAmount} chat areas!");
    }

    protected override bool CheckCompletion()
    {      
        if (VisitedAmount >= NeedToVisitAmount)
        {
            return true;
        }

        return false;
    }

    public override void Reset()
    {
        VisitedAmount = 0;
    }
}
