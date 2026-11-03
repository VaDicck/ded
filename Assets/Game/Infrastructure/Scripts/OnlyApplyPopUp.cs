using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OnlyApplyPopUp : MonoBehaviour
{
    public event Action Applied;
    
    [SerializeField] private Button _applyButton;
    
    [Inject] private InputController _inputController;
    
    private void Start()
    {
        Time.timeScale = 0;
        _inputController.SwitchToUi();
        _applyButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            Applied?.Invoke();
            _inputController.SwitchToGameplay();
        });
    }
}