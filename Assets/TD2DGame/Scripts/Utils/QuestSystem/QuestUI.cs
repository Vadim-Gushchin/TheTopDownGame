using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static Quest;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject qbjectiveTextPrefab;

    //public Quest testQuest;
    //public int testQuestAmount;
    //private List<QuestProgress> testQuests = new();


    void Start()
    {
        //for (int i = 0; i < testQuestAmount; i++)
        //{
        //    testQuests.Add(new QuestProgress(testQuest));
        //}
    }

    public void UpdateQuestUI()
    {
        if (QuestController.Instance == null) return;

        // Clear existing quest entries
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        // Create new quest entries
        // foreach (QuestProgress quest in testQuests)
        foreach (QuestProgress quest in QuestController.Instance.activateQuest)
        {
            GameObject questEntry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = questEntry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = questEntry.transform.Find("ObjectiveList");

            questNameText.text = quest.quest.questName;

            foreach (var objective in quest.questObjectives)
            {
                GameObject objectiveQuest = Instantiate(qbjectiveTextPrefab, objectiveList);
                TMP_Text objectiveQuestText = objectiveQuest.GetComponent<TMP_Text>();
                objectiveQuestText.text = $"{objective.objectiveDescription} ({objective.objectiveCurrentAmount}/{objective.objectiveRequiredAmount})";
            }
        }
    }
}
