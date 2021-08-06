using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestReward", menuName = "Gamerjibe/Questing/QuestReward")]
public class QuestReward : ScriptableObject
{
    public Item Reward;

    // Should be able to give any item as an Award
    public void RewardPlayer()
    {
        PlayerCustomizationManager.Instance.SetAccessory(Reward as AccessoryObject);
        PlayerCustomizationManager.Instance.SaveCustomization();
    }
}
