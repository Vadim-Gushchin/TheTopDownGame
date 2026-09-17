using System;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] private  Slider sfxSlider;
    private static SoundEffectManager Instanse;
    private static AudioSource audioSource;
    private static AudioSource randomPitchAudioSource;
    private static AudioSource voiceAudioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    

    private void Awake()
    {
        if (Instanse == null)
        {
            Instanse = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource = audioSources[0];
            randomPitchAudioSource = audioSources[1];
            voiceAudioSource = audioSources[2];
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
          
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
    }

    public static void PlaySoundEffect(string soundEffectName,bool randomPitch =false)
    {
        AudioClip audioClip = soundEffectLibrary.GetAudioClip(soundEffectName);
        if (audioClip != null)
        {
            if (randomPitch)
            {
                randomPitchAudioSource.pitch = UnityEngine.Random.Range(0.5f, 1.5f);
                randomPitchAudioSource.PlayOneShot(audioClip);
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }
    public static void PlayVoice(AudioClip audioClip, float pitch = 1f,float volume = 1f)
    {
        voiceAudioSource.pitch = pitch;
        voiceAudioSource.volume = volume;
        voiceAudioSource.PlayOneShot(audioClip);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        randomPitchAudioSource.volume = volume;
        voiceAudioSource.volume = volume;
    }

    public  void OnVolumeChange()
    {
        SetVolume(sfxSlider.value);
    }

}
