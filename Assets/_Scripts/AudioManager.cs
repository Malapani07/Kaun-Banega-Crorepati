using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {      
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound S = Array.Find(sounds, sound => sound.name == name);
        if (S == null) { return; };
        S.Source.Play();
    }
    public void Stop(string name)
    {
        Sound S = Array.Find(sounds, sound => sound.name == name);
        if (S == null) { return; };
        S.Source.Stop();
    }
    public void Pause(string name)
    {
        Sound S = Array.Find(sounds, sound => sound.name == name);
        if (S == null) { return; };
        S.Source.Pause();
    }
    public void UnPause(string name)
    {
        Sound S = Array.Find(sounds, sound => sound.name == name);
        if (S == null) { return; };
        S.Source.UnPause();
    }
}
