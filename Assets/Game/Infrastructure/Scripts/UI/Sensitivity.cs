using UnityEngine;
using UnityEngine.UI;
using Zenject;
using R3;
using TMPro;

public class Sensitivity : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _countSensitivity;

    private InputController _inputController;
    
    [Inject]
    private void Init(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Start()
    {
        UpdateText(_inputController.XSensitivity.CurrentValue);
        
        _slider.onValueChanged.AddListener(UpdateText);
        _slider.onValueChanged.AddListener(ApplySensitivity);
    }

    private void UpdateText(float value)
    {
        _countSensitivity.text = value.ToString("F2");
    }

    private void OnEnable()
    {
        _slider.value = _inputController.XSensitivity.CurrentValue;
    }

    private void ApplySensitivity(float value)
    {
        _inputController.SetSensitivity(value, value);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(UpdateText);
        _slider.onValueChanged.RemoveListener(ApplySensitivity);
    }
}
