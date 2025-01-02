using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Sound[] sfxs;
    [SerializeField] private Sound[] musics;
    [SerializeField] private bool isInCutscene = false;

    private int _currentIndex;

    protected override void Awake()
    {
        base.Awake();

        GameObject sfxContainer = Instantiate(new GameObject("SFX"));
        sfxContainer.transform.parent = this.transform;

        foreach (Sound s in sfxs)
        {
            if (s.clip == null)
            {
                Debug.LogError($"Sound '{s.name}' has no audio clip assigned!");
                continue;
            }

            s.source = sfxContainer.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

        }

        GameObject musicContainer = Instantiate(new GameObject("Musics"));
        musicContainer.transform.parent = this.transform;

        foreach (Sound s in musics)
        {
            if (s.clip == null)
            {
                Debug.LogError($"Sound '{s.name}' has no audio clip assigned!");
                continue;
            }

            s.source = musicContainer.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

        }
    }

    private void Start()
    {
        _currentIndex = 0;
        if (!isInCutscene) PlayCurrentMusic();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxs, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError($"Sound '{name}' not found!");
            return;
        }
        if (s.clip == null)
        {
            Debug.LogError($"Audio clip for sound '{name}' is missing!");
            return;
        }

        s.source.Play();

    }

    public void PlayCurrentMusic()
    {
        musics[_currentIndex].source.Play();
        StartCoroutine(WaitForTrackToEnd(musics[_currentIndex].source));
    }

    public void StopMusic()
    {
        musics[_currentIndex].source.Stop();
        StopAllCoroutines();
    }

    private IEnumerator WaitForTrackToEnd(AudioSource audioSource)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        _currentIndex = (_currentIndex + 1) % musics.Length;
        PlayCurrentMusic();
    }
}
