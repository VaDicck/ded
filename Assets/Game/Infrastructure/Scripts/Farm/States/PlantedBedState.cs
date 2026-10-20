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
    
    private readonly GameObject _visualState;
    private readonly GameObject _pouredVisuals;
    private GameObject _prefab;

    public PlantedBedState(GameObject visualState, GameObject pouredVisuals)
    {
        _visualState = visualState;
        _pouredVisuals = pouredVisuals;
    }
    
    public void SetCulture(Culture culture)
    {
        _culture = culture;
        _culture.Collected += OnCollected;
        _culture.IsPoured.Subscribe(OnPoredChanged);
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

    public UniTask<bool> Use(ToolType toolType)
    {
        return UniTask.FromResult(_culture.UseTool(toolType));
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
        _pouredVisuals.SetActive(false);
    }

    private void OnCollected()
    {
        Performed?.Invoke();
    }

    private void OnPoredChanged(bool value)
    {
        _pouredVisuals.SetActive(value);
    }
    
    private void ActivateVisualState(bool isActive)
    {
        _visualState.gameObject.SetActive(isActive);
    }
}