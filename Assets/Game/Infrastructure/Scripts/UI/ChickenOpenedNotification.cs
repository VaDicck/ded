using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChickenOpenedNotification : MonoBehaviour
{
    [SerializeField] private Button _applyButton;

    private InputController _inputController;
    private AudioService _audioService;
    
    [Inject]
    private void Initialize(InputController inputController, AudioService audioService)
    {
        _inputController = inputController;
        _audioService = audioService;
    }

    private void Start()
    {
        _applyButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        _inputController.BackPressed += Hide;
        gameObject.SetActive(true);
        _inputController.SwitchToUi();
        Time.timeScale = 0f;
        _audioService.GameTimeScale.Value = Time.timeScale;
    }

    private void Hide()
    {
        _inputController.BackPressed -= Hide;
        gameObject.SetActive(false);
        _inputController.SwitchToGameplay();
        Time.timeScale = 1f;
        _audioService.GameTimeScale.Value = Time.timeScale;
    }
}