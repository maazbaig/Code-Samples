using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestHelperFunctions
{
    public static List<QuestObjective> CreateObjectivesCopy(List<QuestObjective> objectives)
    {
        List<QuestObjective> questObjectives = new List<QuestObjective>();
        foreach(QuestObjective objective in objectives)
        {
            questObjectives.Add(CloneObjective(objective));
        }

        return questObjectives;
    }

    public static T CloneObjective<T>(this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null)
        {
            Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
            return (T)ScriptableObject.CreateInstance(typeof(T));
        }

        T instance = Object.Instantiate(scriptableObject);
        instance.name = scriptableObject.name; // remove (Clone) from name
        return instance;
    }
}
