using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioService : MonoBehaviour
{
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
    [SerializeField] private AudioMixer _audioMixer;
    
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
        _sfxSource.clip = clip;
        _sfxSource.volume = volume;
        _sfxSource.Play();
    }
    
    public void SetVolume(string group, float volume)
    {
        var value = Mathf.Log10(volume) * 20;

        if (volume == 0)
            value = -80;

        _audioMixer.SetFloat(group, value);
    }
}