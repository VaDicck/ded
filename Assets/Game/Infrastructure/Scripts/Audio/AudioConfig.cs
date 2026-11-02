using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/Audio Config")]
public class AudioConfig : ScriptableObject
{
    [field: SerializeField, Range(0, 1)] public float DefaultMasterVolume { get; private set; }
    [field: SerializeField, Range(0, 1)] public float DefaultMusicVolume { get; private set; }
    [field: SerializeField, Range(0, 1)] public float DefaultSfxVolume { get; private set; }
    
    [field: SerializeField] public string MasterVolumeName { get; private set; }
    [field: SerializeField] public string MusicVolumeName { get; private set; }
    [field: SerializeField] public string SfxVolumeName { get; private set; }
    [field: SerializeField] public string SfxPitchName { get; private set; }
}