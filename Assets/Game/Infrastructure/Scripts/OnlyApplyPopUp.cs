using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OnlyApplyPopUp : MonoBehaviour
{
    public event Action Applied;
    
    [SerializeField] private Button _applyButton;
    
    [Inject] private InputController _inputController;
    [Inject] private AudioService _audioService;
    
    private void Start()
    {
        _applyButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            _audioService.GameTimeScale.Value = Time.timeScale;
            Applied?.Invoke();
            _inputController.SwitchToGameplay();
        });
    }

    private void Update()
    {
        Time.timeScale = 0;
        _audioService.GameTimeScale.Value = Time.timeScale;
        _inputController.SwitchToUi();
    }
}