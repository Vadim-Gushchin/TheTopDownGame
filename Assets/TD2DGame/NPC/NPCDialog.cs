using UnityEngine;

[CreateAssetMenu(fileName ="NewNPCDialog",menuName ="NPC Dialog")]
public class NPCDialog : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogLines;
    public float dialogSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
    public float voiceVolume = 1f;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;

}
