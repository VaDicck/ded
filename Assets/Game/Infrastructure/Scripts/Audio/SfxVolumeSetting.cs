public class SfxVolumeSetting : VolumeSetting
{
    protected override void Start()
    {
        base.Start();
        
        _slider.value = _audioConfig.DefaultSfxVolume;
        UpdateText(_audioConfig.DefaultSfxVolume);
    }

    protected override void ApplyVolume(float value)
    {
        _audioService.SetVolume(_audioConfig.SfxVolumeName, value);
    }
}