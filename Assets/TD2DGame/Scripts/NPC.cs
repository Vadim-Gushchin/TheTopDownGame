using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteracteble
{
    public NPCDialog dialogueData;
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    private string currentFullText;

    private enum QuestState { NotStarted, InProgress, Complited }
    private QuestState questState = QuestState.NotStarted;


    private void Start()
    {
        dialogueUI = DialogueController.Instance;
    }

    private void Update()
    {
        // ✅ Обработка нажатия пробела ТОЛЬКО когда диалог активен
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseController.IsGamePaused && !isDialogueActive))
        {
            return;
        }

        if (!isDialogueActive)
        {
            StartDialogue();
        }
    }
    public void EndDialogue()
    {
        if (questState == QuestState.Complited && !QuestController.Instance.IsQuestHandedIn(dialogueData.quest.questID))
        {
            HandleQuestComplition(dialogueData.quest);
        }

        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        PauseController.SetPause(false);
    }

    void HandleQuestComplition(Quest quest)
    {
        RewardController.Instance.GiveQuestReard(quest);
        QuestController.Instance.HandInQuest(quest.questID);
    }

    private void StartDialogue()
    {
        dialogueIndex = 0;
        SyncQuestState();

        if (questState == QuestState.NotStarted)
        {
            dialogueIndex = 0;
        }
        else if (questState == QuestState.InProgress)
        {
            dialogueIndex = dialogueData.questInProgress;
        }
        else if (questState == QuestState.Complited)
        {
            dialogueIndex = dialogueData.questCompletedIndex;
        }

        isDialogueActive = true;
       

        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);


        dialogueUI.ShowDialogueUI(true);
        PauseController.SetPause(true);

        DisplayCurrentLine();
    }

    private void SyncQuestState()
    {
        if (dialogueData.quest == null) return;

        string questID = dialogueData.quest.questID;

        if (QuestController.Instance.IsQuestCompleted(questID) || QuestController.Instance.IsQuestHandedIn(questID))
        {
            questState = QuestState.Complited;
        }
        else if (QuestController.Instance.IsQuestActive(questID))
        {
            questState = QuestState.InProgress;
        }
        else
        {
            questState = QuestState.NotStarted;
        }
    }
    private void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogLines[dialogueIndex]);
            isTyping = false;
            return;
        }

        //Clear Choices
        dialogueUI.ClearChoices();
        if ((dialogueData.endDialogueLines.Length > dialogueIndex) && (dialogueData.endDialogueLines[dialogueIndex]))
        {
            EndDialogue();
            return;
        }
        foreach (DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if (++dialogueIndex < dialogueData.dialogLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }
    private void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndex[i];
            bool givesQuest = choice.givesQuest[i];
            dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOptionn(nextIndex, givesQuest));
        }
    }

    private void ChooseOptionn(int nexIndex, bool givesQuest)
    {
        if (givesQuest)
        {
            QuestController.Instance.AcceptQuest(dialogueData.quest);
            questState = QuestState.InProgress;
        }
        dialogueIndex = nexIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;

        currentFullText = dialogueData.dialogLines[dialogueIndex];
        string currentTextTemp = "";

        foreach (char letter in currentFullText)
        {
            currentTextTemp += letter;
            dialogueUI.SetDialogueText(currentTextTemp);
            SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch, dialogueData.voiceVolume);
            yield return new WaitForSeconds(dialogueData.dialogSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines != null &&
            dialogueIndex < dialogueData.autoProgressLines.Length &&
            dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }
}

