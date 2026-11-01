using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CultureOpenedNotification : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
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

    public void Show(Sprite sprite, string name)
    {
        _inputController.BackPressed += Hide;
        gameObject.SetActive(true);
        _inputController.SwitchToUi();
        Time.timeScale = 0f;
        _icon.sprite = sprite;
        _name.text = name;
    }

    private void Hide()
    {
        _inputController.BackPressed -= Hide;
        gameObject.SetActive(false);
        _inputController.SwitchToGameplay();
        Time.timeScale = 1f;
    }
}
