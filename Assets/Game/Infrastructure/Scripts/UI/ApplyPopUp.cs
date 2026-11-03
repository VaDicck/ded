using System;
using UnityEngine;
using UnityEngine.UI;

public class ApplyPopUp : MonoBehaviour
{
    public event Action Applied;
    
    [SerializeField] private Button _applyButton;

    private void Start()
    {
        _applyButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            Applied?.Invoke();
        });
    }
}
