using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Transform _respawnPlayer;
    [SerializeField] private Button _stuckButton;
    
    private PlayerMovement _playerMovement;
    private InputController _inputController;
    
    [Inject]
    private void Init(InputController inputController, PlayerMovement playerMovement)
    {
        _inputController = inputController;
        _playerMovement = playerMovement;
    }

    private void Start()
    {
        _inputController.BackPressed += () =>
        {
            this.Log("НАЖАЛИ");
        };
            
        _stuckButton.onClick.AddListener(OnStuckButtonClicked);

        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void OnStuckButtonClicked()
    {
        _playerMovement.transform.position = _respawnPlayer.position;
    }

    private void OnDestroy()
    {
        _stuckButton.onClick.RemoveListener(OnStuckButtonClicked);
    }
}
