using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialog", menuName = "NPC Dialog")]
public class NPCDialog : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public float dialogSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public float voiceVolume = 1f;
    public float autoProgressDelay = 1.5f;
    public string[] dialogLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;

    public DialogueChoice[] choices;

    public int questInProgress;
    public int questCompletedIndex;
    public Quest quest;
}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;
    public string[] choices;
    public int[] nextDialogueIndex;
    public bool[] givesQuest;
}
