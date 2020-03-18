using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager current = null;

    [SerializeField] private bool playMusic;
    [SerializeField] private bool playSFX;

    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);

        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource2.loop = true;

        musicSource.volume = musicVolume;
        musicSource2.volume = musicVolume;

        sfxSource.volume = sfxVolume;
    }
    #endregion

    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private bool firstSourceBusy;
    private float musicVolume   = 0.15f;
    private float sfxVolume     = 1.0f;

    public void PlayMusic(AudioClip musicClip)
    {
        if (!playMusic)
            return;

        AudioSource activeSource = (firstSourceBusy) ? musicSource : musicSource2;

        activeSource.clip = musicClip;
        activeSource.volume = musicVolume;
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

    public void SetMusicVolume(float volume)
    {
        if (!playSFX)
            return;

        musicVolume = volume;

        musicSource.volume = musicVolume;
        musicSource2.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        if (!playSFX)
            return;

        sfxVolume = volume;

        sfxSource.volume = sfxVolume;
    }
}
