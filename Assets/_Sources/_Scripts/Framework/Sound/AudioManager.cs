using Framework.DesignPattern.Singleton;

namespace Framework.Audio
{
    using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Audio
{
    public AudioType audioType;
    public List<AudioClip> clip;
    public TargetOption targetOption;
    [Range(0f, 1f)] public float volume = 1;
    [Range(.1f, 3f)] public float pitch = 1;
    public bool loop;

    private AudioClip _cache;
    
    public AudioClip GetClip(bool getCache = false)
    {
        try
        {
            AudioClip res;
            if (getCache && _cache != null)
            {
                res = _cache;
            }
            else
            {
                res = clip[targetOption == TargetOption.First ? 0 : UnityEngine.Random.Range(0, clip.Count)];
                _cache = res;
            }

            return res;
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogError("Audio clip is empty");
            return null;
        }
    }
}

public enum TargetOption
{
    First,
    Random
}

public enum AudioType
{
    Sound,
    Music
}


public class AudioManager : Singleton<AudioManager>
{
    private List<AudioSource> _audioSource;
    private List<AudioStatus> _status;

    public override void Awake()
    {
        base.Awake();
        _audioSource = new List<AudioSource>();
        _status = new List<AudioStatus>();
    }

    public static bool IsPlaying(AudioIndex index)
    {
        foreach (var source in Instance._audioSource)
        {
            if (source.clip != AudioData.GetAudio(index).GetClip(true)) continue;
            if (source.isPlaying) return true;
        }

        return false;
    }
    
    public void ChangePitch(float pitch)
    {
        foreach (var source in _audioSource)
        {
            source.pitch = pitch;
        }
    } 
    
    public static void PlayOneShot(AudioIndex index)
    {
        var audio = AudioData.GetAudio(index);
        // Debug.Log(index.ToString());
        if (!Instance.Playable(audio)) return;
        Instance.GetAudioSourceAvailable().PlayOneShot(audio.GetClip());
    }
    
    public static void Play(AudioIndex index)
    {
        AudioSource source = Instance.GetAudioSourceAvailable();
        Audio audio = AudioData.GetAudio(index);
        if (Instance.Playable(audio))
        {
            source.Setup(audio).Play();
        }
    }
    
    public static void Stop(AudioIndex index)
    {
        Debug.Log("Stop " + index);
        foreach (AudioSource source in Instance._audioSource)
        {
            if (source.clip == AudioData.GetAudio(index).GetClip(true))
            {
                source.Stop();
            }
        }
    }
    
    private AudioSource GetAudioSourceAvailable()
    {
        foreach (var audioSource in _audioSource)
        {
            if (!audioSource.isPlaying || audioSource.clip == null)
            {
                return audioSource;
            }
        }
        var source = gameObject.AddComponent<AudioSource>();
        _audioSource.Add(source);
        _status.Add(new AudioStatus());
        return source;
    }
    
    private bool Playable(Audio audio)
    {
        return audio.audioType switch {
            AudioType.Sound => PlayerPrefs.GetInt(AudioType.Sound.ToString(), 1) == 1,
            _ => PlayerPrefs.GetInt(AudioType.Music.ToString(), 1) == 1,
        };
    }
}

public class AudioStatus
{
    public AudioIndex ID;
    public bool IsPlaying;
}

public static class AudioExtension
{
    public static AudioSource Setup(this AudioSource audioSource, Audio audio)
    {
        audioSource.clip = audio.GetClip();
        audioSource.volume = audio.volume;
        audioSource.pitch = audio.pitch;
        audioSource.loop = audio.loop;

        return audioSource;
    }
}
}