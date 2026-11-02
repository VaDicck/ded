public class MusicVolumeSetting : VolumeSetting
{
    protected override void Start()
    {
        base.Start();

        _slider.value = _audioConfig.DefaultMusicVolume;
        UpdateText(_audioConfig.DefaultMusicVolume);
    }

    protected override void ApplyVolume(float value)
    {
        _audioService.SetVolume(_audioConfig.MusicVolumeName, value);
    }
}