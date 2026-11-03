public class MasterVolumeSetting : VolumeSetting
{
    protected override void Start()
    {
        base.Start();
        
        _slider.value = _audioService.MasterVolume;
    }

    protected override void ApplyVolume(float value)
    {
        _audioService.SetVolume(_audioConfig.MasterVolumeName, value);
    }
}