public class SfxVolumeSetting : VolumeSetting
{
    protected override void Start()
    {
        base.Start();
        
        _slider.value = _audioService.SfxVolume;
    }

    protected override void ApplyVolume(float value)
    {
        _audioService.SetVolume(_audioConfig.SfxVolumeName, value);
    }
}