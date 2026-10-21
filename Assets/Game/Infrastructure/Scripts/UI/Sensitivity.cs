using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Sensitivity : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _applyButton;

    private PlayerMovement _movement;

    [Inject]
    private void Init(PlayerMovement playerMovement)
    {
        _movement = playerMovement;
    }

    private void Start()
    {
        _applyButton.onClick.AddListener(ApplySensitivity);
    }

    private void ApplySensitivity()
    {
        _movement.SetDefaultSensitivity(_slider.value, _slider.value);
    }

    private void OnDestroy()
    {
        _applyButton.onClick.RemoveListener(ApplySensitivity);
    }
}
