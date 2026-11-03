using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public abstract class VolumeSetting : MonoBehaviour
{
    [SerializeField] protected Slider _slider;
    [SerializeField] private TMP_Text _currentCount;

    protected AudioService _audioService;
    protected AudioConfig _audioConfig;
    
    [Inject]
    private void Init(AudioService audioService, GameConfig gameConfig)
    {
        _audioService = audioService;
        _audioConfig = gameConfig.AudioConfig;
    }

    protected virtual void Start()
    {
        _slider.onValueChanged.AddListener(UpdateText);
        _slider.onValueChanged.AddListener(ApplyVolume);
    }

    protected void UpdateText(float value)
    {
        float percent = (value / _slider.maxValue) * 100f;
    
        _currentCount.text = $"{Mathf.RoundToInt(percent)}%";
    }

    protected abstract void ApplyVolume(float value);

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(UpdateText);
        _slider.onValueChanged.RemoveListener(ApplyVolume);
    }
}