public class MusicVolumeSetting : VolumeSetting
{
    protected override void Start()
    {
        base.Start();

        _slider.value = _audioService.MusicVolume;
    }

    protected override void ApplyVolume(float value)
    {
        _audioService.SetVolume(_audioConfig.MusicVolumeName, value);
    }
}