using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class AudioService : IInitializable
{
    private readonly AudioConfig _config;
    private readonly Dictionary<string, AudioClip> _sfxMap = new();
    private readonly Dictionary<string, AudioClip> _musicMap = new();

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    public AudioService(AudioConfig config)
    {
        _config = config;
    }

    public void Initialize()
    {
        var audioGo = new GameObject("AudioService");
        Object.DontDestroyOnLoad(audioGo);

        _musicSource = audioGo.AddComponent<AudioSource>();
        _musicSource.loop = true;

        _sfxSource = audioGo.AddComponent<AudioSource>();

        foreach (var entry in _config.Sfx)
        {
            if (!string.IsNullOrEmpty(entry.Key))
                _sfxMap[entry.Key] = entry.Value;
        }

        foreach (var entry in _config.Music)
        {
            if (!string.IsNullOrEmpty(entry.Key))
                _musicMap[entry.Key] = entry.Value;
        }
    }

    public void PlaySFX(string key)
    {
        if (_sfxMap.TryGetValue(key, out var clip))
            _sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(string key)
    {
        if (!_musicMap.TryGetValue(key, out var clip)) return;
        
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void StopMusic() => _musicSource.Stop();
}