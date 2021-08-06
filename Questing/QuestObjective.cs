using UnityEngine;

public abstract class QuestObjective : ScriptableObject
{
    private Quest Quest;
    private bool Completed;

    public void Complete()
    {
        if (GetQuest() == null || GetQuestStatus() != QuestStatus.Started) return;

        IncrementProgress();

        if (CheckCompletion())
            CompleteObjective();
    }

    protected abstract void IncrementProgress();

    protected abstract bool CheckCompletion();

    public abstract void Reset();

    public bool IsCompleted()
    {
        return Completed;
    }

    public void CompleteObjective()
    {
        Completed = true;
        Quest.CompleteObjective();
    }

    public void SetQuest(Quest quest)
    {
        Quest = quest;
    }

    public Quest GetQuest()
    {
        return Quest;
    }

    public QuestStatus GetQuestStatus()
    {
        return Quest.GetQuestStatus();
    }

}
