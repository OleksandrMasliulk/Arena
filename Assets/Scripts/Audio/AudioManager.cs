using UnityEngine.Audio;
using System;
using UnityEngine;
using static Sound;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        Clear();
        DontDestroyOnLoad(this);
        InitVolume();
        Play("Theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void InitSound(GameObject obj, string name)
    {
        AudioSource source = obj.AddComponent<AudioSource>();
        Sound s = Array.Find(sounds, sound => sound.name == name);

        source.clip = s.clip;

        source.volume = s.volume;
        source.pitch = s.pitch;
        source.loop = s.loop;
    }

    public void Play(GameObject obj)
    {
        obj.GetComponent<AudioSource>().Play();
    }

    public void StopPlaying(GameObject obj)
    {
        obj.GetComponent<AudioSource>().Stop();
    }

    public void StopPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void SetSoundsVolume(float volume)
    {
        foreach (Sound sound in sounds)
            if (sound.type == SoundType.Sound)
                sound.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        foreach (Sound sound in sounds)
            if (sound.type == SoundType.Music)
            {
                sound.volume = volume;
                sound.source.volume = volume;
            }
    }

    void InitVolume()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.type == SoundType.Sound)
            {
                sound.volume = PlayerPrefs.GetFloat("SoundsVolume", .5f);
            }
            else
            {
                sound.volume = PlayerPrefs.GetFloat("MusicVolume", .5f);
            }
        }
    }

    void Clear()
    {
        AudioManager[] managers = FindObjectsOfType<AudioManager>();

        if (managers.Length > 1)
            Destroy(this.gameObject);
    }
}
