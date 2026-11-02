using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChickenOpenedNotification : MonoBehaviour
{
    [SerializeField] private Button _applyButton;

    private InputController _inputController;
    
    [Inject]
    private void Initialize(InputController inputController)
    {
        _inputController = inputController;
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
    }

    private void Hide()
    {
        _inputController.BackPressed -= Hide;
        gameObject.SetActive(false);
        _inputController.SwitchToGameplay();
        Time.timeScale = 1f;
    }
}