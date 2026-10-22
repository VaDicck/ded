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
    }

    private void Start()
    {
        _inputController.BackPressed += OnEscClicked;
        _stuckButton.onClick.AddListener(OnStuckButtonClicked);
        _continueButton.onClick.AddListener(Hide);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
        
        Hide();
    }

    private void OnEscClicked()
    {
        if (_isPaused)
            Hide();
        else
            Show();
    }

    private void Hide()
    {
        Time.timeScale = 1;
        
        _isPaused = false;
        gameObject.SetActive(false);
        
        _inputController.SwitchToGameplay();
    }
    
    private void Show()
    {
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
        _inputController.BackPressed -= OnEscClicked;
    }
}
