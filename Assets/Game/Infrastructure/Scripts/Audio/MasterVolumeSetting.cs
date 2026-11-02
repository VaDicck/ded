public class MasterVolumeSetting : VolumeSetting
{
    protected override void Start()
    {
        base.Start();
        
        _slider.value = _audioConfig.DefaultMasterVolume;
        UpdateText(_audioConfig.DefaultSfxVolume);
    }

    protected override void ApplyVolume(float value)
    {
        _audioService.SetVolume(_audioConfig.MasterVolumeName, value);
    }
}