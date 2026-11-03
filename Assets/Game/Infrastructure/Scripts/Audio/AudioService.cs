using System;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using R3;

public class AudioService : MonoBehaviour
{
    public readonly ReactiveProperty<float> GameTimeScale = new ReactiveProperty<float>(1f);
    
    public float MasterVolume
    {
        get
        {
            _audioMixer.GetFloat(_config.MasterVolumeName, out var volume);

            return Mathf.Pow(10, volume / 20);
        }
    }
    
    public float MusicVolume
    {
        get
        {
            _audioMixer.GetFloat(_config.MusicVolumeName, out var volume);

            return Mathf.Pow(10, volume / 20);
        }
    }

    public float SfxVolume
    {
        get
        {
            _audioMixer.GetFloat(_config.SfxVolumeName, out var volume);

            return Mathf.Pow(10, volume / 20);
        }
    }
    
    private AudioConfig _config;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _uiSfxSource;
    [SerializeField] private AudioMixer _audioMixer;
    
    private IDisposable _timeScaleSubscription;
    
    [Inject]
    private void Init(GameConfig gameConfig) 
    {
        _config = gameConfig.AudioConfig;
    }

    private void Start()
    {
        _audioMixer.SetFloat(_config.MasterVolumeName, _config.DefaultMasterVolume);
        _audioMixer.SetFloat(_config.MusicVolumeName, _config.DefaultMusicVolume);
        _audioMixer.SetFloat(_config.SfxVolumeName, _config.DefaultSfxVolume);
        
        _timeScaleSubscription = GameTimeScale.Subscribe(ApplySfxPitch);
    }
    
    private void ApplySfxPitch(float scale)
    {
        _audioMixer.SetFloat(_config.SfxPitchName, scale);

        if (scale <= 0f)
            _sfxSource.Pause();
        else
            _sfxSource.UnPause();
    }
    
    public void PlayMusic(AudioClip clip, float volume = 1)
    {
        _musicSource.clip = clip;
        _musicSource.volume = volume;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlaySfx(AudioClip clip, float volume = 1)
    {
        if (GameTimeScale.Value <= 0f) return;
        
        _sfxSource.volume = volume;
        _sfxSource.PlayOneShot(clip);
    }
    
    public void PlayUiSfx(AudioClip clip, float volume = 1)   
    {
        _uiSfxSource.volume = volume;
        _uiSfxSource.PlayOneShot(clip);
    }
    
    public void SetVolume(string group, float volume)
    {
        var value = Mathf.Log10(volume) * 20;

        if (volume == 0)
            value = -80;

        _audioMixer.SetFloat(group, value);
    }
    
    private void OnDestroy()
    {
        _timeScaleSubscription?.Dispose();
    }
}