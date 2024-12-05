using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private Sound[] sounds;

    protected override void Awake()
    {
        base.Awake();
        foreach (Sound s in sounds)
        {
            if (s.clip == null)
            {
                Debug.LogError($"Sound '{s.name}' has no audio clip assigned!");
                continue;
            }

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
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

        Debug.Log(name + " is playing");
        s.source.Play();

    }
}
