using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlantedBedState : IBedState
{
    public event Action Performed;

    public bool IsInteractable => _culture.IsGrown.CurrentValue;
    public bool IsUsable => _culture.IsGrown.CurrentValue == false;
    
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
        _culture.IsGrown.Subscribe(v =>
        {
            if (v) this.Log($"{_culture.Type} was grown");
        });
        _prefab = Object.Instantiate(_culture.CultureConfig.Step1Prefab, _visualState.transform);
    }

    public UniTask Interact()
    {
        _culture.Collect();
        
        return UniTask.CompletedTask;
    }
    
    public void Enter()
    {
        ActivateVisualState(true);
        this.Log($"Entered with culture {_culture}");
    }
    
    public void Exit()
    {
        if (_culture != null)
        {
            _culture.Collected -= OnCollected;
        }

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