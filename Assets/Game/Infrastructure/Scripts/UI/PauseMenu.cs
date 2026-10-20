using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Transform _respawnPlayer;
    [SerializeField] private Button _stuckButton;
    
    private PlayerReturner _playerReturner;
    private InputController _inputController;
    
    private bool _isPaused = false;
    
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

        Hide();
    }

    private void OnEscClicked()
    {
        if (_isPaused)
        {
            Hide();
            _inputController.SwitchToGameplay();
        }
        else
        {
            Show();
            _inputController.SwitchToUi();
        }
    }

    private void Hide()
    {
        Time.timeScale = 1;
        
        _isPaused = false;
        gameObject.SetActive(false);
    }
    
    private void Show()
    {
        Time.timeScale = 0;
        
        _isPaused = true;
        gameObject.SetActive(true);
    }
    
    private void OnStuckButtonClicked()
    {
        _playerReturner.Return();
    }

    private void OnDestroy()
    {
        _stuckButton.onClick.RemoveListener(OnStuckButtonClicked);
    }
}
