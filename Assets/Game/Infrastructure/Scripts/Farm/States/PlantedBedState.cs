using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlantedBedState : IBedState
{
    public event Action Performed;

    public bool IsInteractable => _culture.IsGrown;
    public bool IsUsable => _culture.IsGrown == false;
    
    public Culture Culture => _culture;

    private Culture _culture;
    
    private GameObject _visualState;
    private GameObject _prefab;

    public PlantedBedState(GameObject visualState)
    {
        _visualState = visualState;
    }
    
    public void SetCulture(Culture culture)
    {
        _culture = culture;
        _culture.Collected += OnCollected;
        _prefab = Object.Instantiate(_culture.CultureConfig.Step1Prefab, _visualState.transform);
        this.Log("hhvmb");
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

        Object.Destroy(_prefab);

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