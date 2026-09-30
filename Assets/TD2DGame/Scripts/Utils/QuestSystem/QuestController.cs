using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using static Quest;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    public List<QuestProgress> activateQuest = new();
    private QuestUI questUI;

    public List<string> handingQuestIDs = new();

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(gameObject);

        questUI = FindObjectOfType<QuestUI>();
        InventoryController.Instance.OnInventoryChanged += CheckInvetoryForQuests;
    }

    public void CheckInvetoryForQuests()
    {
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();

        foreach (QuestProgress quest in activateQuest)
        {
            foreach (QuestObjectives questObjectives in quest.questObjectives)
            {
                if (questObjectives.type != Quest.ObjectiveType.CollectionItem) continue;
                if (!int.TryParse(questObjectives.objectiveID, out int itemId)) continue;

                int newAmount = itemCounts.TryGetValue(itemId, out int count) ? Math.Min(count, questObjectives.objectiveRequiredAmount) : 0;

                if (questObjectives.objectiveCurrentAmount != newAmount)
                {
                    questObjectives.objectiveCurrentAmount = newAmount;
                }
            }
        }
        questUI.UpdateQuestUI();
    }


    public void AcceptQuest(Quest quest)
    {
        if (IsQuestActive(quest.questID)) { return; }

        activateQuest.Add(new QuestProgress(quest));

        CheckInvetoryForQuests();
        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(string questID) => activateQuest.Exists(quest => quest.questID == questID);

    public bool IsQuestCompleted(string questID)
    {
        QuestProgress quest = GetQuestByID(questID);
        return quest != null && quest.questObjectives.TrueForAll(objectivesTemp => objectivesTemp.IsComplited);
    }

    public void HandInQuest(string questID)
    {
        if (!RemoveRequredItemsFromInvetory(questID))
        {
            return;
        }
        QuestProgress quest = GetQuestByID(questID);
        if (quest != null)
        {
            handingQuestIDs.Add(quest.questID);
            activateQuest.Remove(quest);
            questUI.UpdateQuestUI();
        }
    }

    public bool IsQuestHandedIn(string questID) => handingQuestIDs.Contains(questID);
    public bool RemoveRequredItemsFromInvetory(string questID)
    {
        QuestProgress quest = GetQuestByID(questID);
        if (quest == null) return false;
        Dictionary<int, int> requiredItems = new();

        foreach (QuestObjectives objectives in quest.questObjectives)
        {
            if (objectives.type == ObjectiveType.CollectionItem && int.TryParse(objectives.objectiveID, out int itemId))
            {
                requiredItems[itemId] = objectives.objectiveRequiredAmount;
            }
        }

        Dictionary<int,int> itemCount = InventoryController.Instance.GetItemCounts();
        foreach(var item in requiredItems)
        {
            if (itemCount.GetValueOrDefault(item.Key) < item.Value)
            {
                return false;
            }
        }

        foreach (var itemRequirment in requiredItems)
        {
            InventoryController.Instance.ReRemoveItemFromInventory(itemRequirment.Key, itemRequirment.Value);
        }

        return true;
    }
    public void LoadQuestProgress(List<QuestProgress> savedQuests)
    {
        activateQuest = savedQuests ?? new();
        if (questUI != null)
        {
            CheckInvetoryForQuests();
            questUI.UpdateQuestUI();
        }
    }

    public QuestProgress GetQuestByID(string questID)
    {
        QuestProgress quest = activateQuest.Find(q => q.questID == questID);

        if (quest == null)
        {
            Debug.LogWarning($"[Quests] Квест с ID {questID} не найден среди активных!");
        }
        return quest;
    }
}



