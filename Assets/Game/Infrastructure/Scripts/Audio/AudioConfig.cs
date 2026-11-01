using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/Audio Config")]
public class AudioConfig : ScriptableObject
{
    [Header("MUSIC")]
    public SerializedPair<string, AudioClip>[] Music;
    
    [Header("SFX")]
    public SerializedPair<string, AudioClip>[] Sfx;
    
}