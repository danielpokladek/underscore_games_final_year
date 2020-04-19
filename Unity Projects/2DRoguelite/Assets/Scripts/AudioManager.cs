using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private bool playMusic;
    [SerializeField] private bool playSFX;
    [Range(0, 1)]
    [SerializeField] private float musicVolume   = 0.15f;
    [Range(0, 1)]
    [SerializeField] private float sfxVolume     = 1.0f;

    #region Singleton
    public static AudioManager current = null;
    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource2.loop = true;

        musicSource.volume = 1;
        musicSource2.volume = 1;
        sfxSource.volume = 1;

        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        musicSource2.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music2")[0];
        sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
    }
    #endregion

    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private bool firstSourceBusy;

    public void PlayMusic(AudioClip musicClip)
    {
        if (!playMusic)
            return;

        AudioSource activeSource = (firstSourceBusy) ? musicSource : musicSource2;

        activeSource.clip = musicClip;
        activeSource.Play();
    }

    public void FadeToNewMusicClip(AudioClip newClip, float fadeOut = 1.0f, float fadeIn = 1.0f)
    {
        if (!playMusic)
            return;

        AudioSource activeSource = (firstSourceBusy) ? musicSource : musicSource2;

        StartCoroutine(FadeMusicClips(activeSource, newClip, fadeOut, fadeIn));
    }

    public void CrossFadeMusicClips(AudioClip newClip, float fadeTime = 1.0f)
    {
        if (!playMusic)
            return;

        AudioSource activeSource = (firstSourceBusy) ? musicSource : musicSource2;
        AudioSource secondSource = (firstSourceBusy) ? musicSource2 : musicSource;

        firstSourceBusy = !firstSourceBusy;

        secondSource.clip = newClip;
        secondSource.Play();

        StartCoroutine(CrossFadeClips(activeSource, secondSource, fadeTime));
    }

    private IEnumerator FadeMusicClips(AudioSource activeSource, AudioClip newClip, float fadeOut, float fadeIn)
    {
        if (!activeSource.isPlaying)
            activeSource.Play();

        float t = 0.0f;


        // Fade out old track.
        for (t = 0; t < fadeOut; t += Time.deltaTime)
        {
            activeSource.volume = (musicVolume - (t / fadeOut) * musicVolume);
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        for (t = 0; t < fadeIn; t += Time.deltaTime)
        {
            activeSource.volume = (t / fadeIn) * musicVolume;
            yield return null;
        }
    }

    private IEnumerator CrossFadeClips(AudioSource firstSource, AudioSource secondSource, float fadeTime)
    {
        float t = 0.0f;

        for (t = 0.0f; t <= fadeTime; t += Time.deltaTime)
        {
            firstSource.volume = (musicVolume - (t / fadeTime) * musicVolume);
            secondSource.volume = (t / fadeTime) * musicVolume;
            yield return null;
        }

        firstSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!playSFX)
            return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        if (!playMusic)
            return;

        audioMixer.SetFloat("MusicVolume", value);
        audioMixer.SetFloat("Music2Volume", value);
    }

    public void SetSFXVolume(float value)
    {
        if (!playSFX)
            return;

        audioMixer.SetFloat("SFXVolume", value);
    }
}
