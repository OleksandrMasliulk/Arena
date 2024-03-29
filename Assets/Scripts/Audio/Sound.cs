﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum SoundType
    {
        Music,
        Sound
    }

    public string name;
    public SoundType type;

    public AudioClip clip;

    public float volume;
    public float pitch;
    public bool loop;

    public AudioSource source;
}
