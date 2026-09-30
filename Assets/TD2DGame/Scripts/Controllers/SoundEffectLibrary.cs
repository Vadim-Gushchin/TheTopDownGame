
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    [System.Serializable]
    public struct SoundEffectGroup
    {
        public string groupName;
        public List<AudioClip> soundEffects;
    }

    public AudioClip GetAudioClip(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundDictionary[name];
            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }

    private void Awake()
    {
        InitializeSoundDictionary();
    }



    private void InitializeSoundDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();
        foreach (SoundEffectGroup soundEffectGroup in soundEffectGroups)
        {
            soundDictionary[soundEffectGroup.groupName] = soundEffectGroup.soundEffects;
        }
    }

}