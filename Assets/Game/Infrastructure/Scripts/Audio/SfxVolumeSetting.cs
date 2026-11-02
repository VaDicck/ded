public class SfxVolumeSetting : VolumeSetting
{
    protected override void Start()
    {
        base.Start();
        
        UpdateText(_audioConfig.DefaultSfxVolume);
    }

    protected override void ApplyVolume(float value)
    {
        _audioService.SetVolume(_audioConfig.SfxVolumeName, value);
    }
}