using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Quest;
using static QuestProgress;


[CreateAssetMenu(menuName = "Quest/Quests")]
public class Quest : ScriptableObject
{

    public enum ObjectiveType
    {
        CollectionItem,
        DefeatEnemy,
        ReachLocation,
        TalkNPC,
        Custom
    }

    public string questID;
    public string questName;
    public string questDescription;
    public List<QuestObjectives> questObjectives;
    public List<QuestReward> questRewards;


    private void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();

        }
    }

}
[System.Serializable]
public class QuestObjectives
{
    public string objectiveID; //   this ID compare with quest target ID  
    public string objectiveDescription;
    public int objectiveRequiredAmount;
    public int objectiveCurrentAmount;
    public ObjectiveType type;

    public bool IsComplited => objectiveCurrentAmount >= objectiveRequiredAmount;
}

[System.Serializable]
public class QuestProgress
{
    public Quest quest;
    public List<QuestObjectives> questObjectives;

    public QuestProgress(Quest quest)
    {
        this.quest = quest;
        questObjectives = new List<QuestObjectives>();

        foreach (var obj in quest.questObjectives)
        {
            questObjectives.Add(new QuestObjectives
            {
                objectiveID = obj.objectiveID,
                objectiveDescription = obj.objectiveDescription,
                objectiveRequiredAmount = obj.objectiveRequiredAmount,
                objectiveCurrentAmount = 0,
                type = obj.type,
            });

        }
    }

    public bool IsComplited => questObjectives.TrueForAll(o => o.IsComplited);

    public string questID => quest.questID;
    public string questName => quest.questName;
}

[System.Serializable]
public class QuestReward
{
    public RewaedType rewardType;
    public int rewardID;
    public int amount = 1;
}

public enum RewaedType
{
    Gold,
    Item,
    Expirience,
    Custom
}