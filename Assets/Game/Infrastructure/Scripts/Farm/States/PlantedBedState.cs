using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlantedBedState : IBedState
{
    public event Action Performed;

    public bool IsInteractable => true;
    public bool IsUsable => false;
    
    public Culture Culture => _culture;

    private Culture _culture;
    
    private GameObject _visualState;

    public PlantedBedState(GameObject visualState)
    {
        _visualState = visualState;
    }
    
    public void SetCulture(Culture culture)
    {
        _culture = culture;
        _culture.Collected += OnCollected;
    }

    public UniTask Interact()
    {
        _culture.Collect();
        
        return UniTask.CompletedTask;
    }
    
    public void Enter()
    {
        ActivateVisualState(true);
        Debug.Log("Entered");
    }
    
    public void Exit()
    {
        if (_culture != null)
            _culture.Collected -= OnCollected;
        
        ActivateVisualState(false);
    }

    private void OnCollected()
    {
        Performed?.Invoke();
    }
    
    private void ActivateVisualState(bool isActive)
    {
        _visualState.gameObject.SetActive(isActive);
    }
}