using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;

public class AudioController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource SFX;
    public AudioClip buttonClick;

    public AudioSource ambientMusic;
    public AudioSource battleMusic;
    public AudioSource bossMusic;
    public AudioSource deathMusic;
    public AudioSource mainMenuMusic;
    private AudioSource[] allMusic;

    [Header("Settings Canvas")]
    public Slider musicVolumeSliderSettings;
    public TextMeshProUGUI musicVolumeTextSettings;

    [Header("Pause Canvas")]
    public Slider musicVolumeSliderPause;
    public TextMeshProUGUI musicVolumeTextPause;

    public float maxMusicVolumeDb;
    public float minMusicVolumeDb;
    private string musicVolumeKey = "musicVolume";
    private float scaledMusicVolume;

    private void Awake()
    {
        scaledMusicVolume = PlayerPrefs.GetFloat(musicVolumeKey, 0);
        allMusic = new AudioSource[] { ambientMusic, battleMusic, bossMusic, deathMusic, mainMenuMusic };
    }

    private void Update()
    {
        audioMixer.SetFloat(musicVolumeKey, scaledMusicVolume);
        float normalizedVolume = GetNormalizedVolume(scaledMusicVolume);

        musicVolumeTextSettings.text = normalizedVolume.ToString("0.00");
        musicVolumeSliderSettings.value = normalizedVolume;

        musicVolumeTextPause.text = normalizedVolume.ToString("0.00");
        musicVolumeSliderPause.value = normalizedVolume;
    }

    public void OnButtonClick()
    {
        SFX.PlayOneShot(buttonClick);
    }

    // Takes a normalized volume value between 0 and 1
    public void SetVolume(float volume)
    {
        float scaledVolume = GetScaledVolume(volume);
        scaledMusicVolume = scaledVolume;
        PlayerPrefs.SetFloat(musicVolumeKey, scaledVolume);
    }

    float GetNormalizedVolume(float volume)
    {
        return (volume  - minMusicVolumeDb) / (maxMusicVolumeDb - minMusicVolumeDb);
    }

    float GetScaledVolume(float volume)
    {
        return volume * (maxMusicVolumeDb - minMusicVolumeDb) + minMusicVolumeDb;
    }

    public void PlayMusic(AudioSource music, bool fade = false)
    {
        if (fade)
        {
            float currentVolume = music.volume;
            music.volume = 0;
            music.Play();
            StartCoroutine(StartAudioFade(music, 1, currentVolume));
        }
        else
        {
            music.Play();
        }
    }

    public void StopAllMusic(bool pause, bool fade = false)
    {
        foreach (AudioSource music in allMusic)
        {
            if (fade)
            {
                if (pause)
                {
                    StartCoroutine(StartAudioFade(music, 1, 0, true));
                }
                else
                {
                    StartCoroutine(StartAudioFade(music, 1, 0, false));
                }
            }
            else
            {
                if (pause)
                {
                    music.Pause();
                }
                else
                {
                    music.Stop();
                }
            }
        }
    }

    public AudioSource GetCurrentMusic()
    {
        foreach (AudioSource music in allMusic)
        {
            if (music.isPlaying)
            {
                return music;
            }
        }

        return null;
    }

    public static IEnumerator StartAudioFade(AudioSource audioSource, float duration, float targetVolume, bool pause = false)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        if (audioSource.volume == 0)
        {
            if (pause)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Stop();
            }
        }
        yield break;
    }
}
