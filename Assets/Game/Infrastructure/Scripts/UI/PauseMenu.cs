using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Transform _respawnPlayer;
    [SerializeField] private Button _stuckButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;
    
    private PlayerReturner _playerReturner;
    private InputController _inputController;
    
    private bool _isPaused;
    
    [Inject]
    private void Init(InputController inputController, PlayerReturner playerReturner)
    {
        _inputController = inputController;
        _playerReturner = playerReturner;
        
        _inputController.PausePressed += Show;
        _stuckButton.onClick.AddListener(OnStuckButtonClicked);
        _continueButton.onClick.AddListener(Hide);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Hide()
    {
        _inputController.BackPressed -= Hide;
        Time.timeScale = 1;
        
        _isPaused = false;
        gameObject.SetActive(false);
        
        _inputController.SwitchToGameplay();
    }
    
    private void Show()
    {
        _inputController.BackPressed += Hide;
        Time.timeScale = 0;
        
        _isPaused = true;
        gameObject.SetActive(true);
        
        _inputController.SwitchToUi();
    }
    
    private void OnStuckButtonClicked()
    {
        _playerReturner.Return();
        Hide();
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
    
    private void OnDestroy()
    {
        _stuckButton.onClick.RemoveListener(OnStuckButtonClicked);
        _continueButton.onClick.RemoveListener(Hide);
        _inputController.PausePressed += Show;
    }
}
