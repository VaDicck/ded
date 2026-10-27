using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlantedBedState : IBedState
{
    public event Action Performed;
    public event Action Watered;
    public event Action Grown;
    public event Action Collected;

    public bool IsInteractable => _culture.IsGrown.CurrentValue;
    public bool IsUsable => _culture.IsGrown.CurrentValue == false;
    public ToolType UseTool
    {
        get {
            if (_culture == null)
                return ToolType.None;
            
            else if (_culture.IsPoured.CurrentValue == false)
                return ToolType.Bailer;
            
            else if(_culture is MutatedCulture && _culture.IsPoured.CurrentValue)
                return ToolType.Mutagen;
            
            return ToolType.None;
        }
    }

    public Culture Culture => _culture;

    private Culture _culture;
    
    private readonly GameObject _visualState;
    private readonly GameObject _pouredVisuals;
    private GameObject _prefab;

    private IDisposable _pouredReaction;
    private IDisposable _grownReaction;

    public PlantedBedState(GameObject visualState, GameObject pouredVisuals)
    {
        _visualState = visualState;
        _pouredVisuals = pouredVisuals;
    }
    
    public void SetCulture(Culture culture)
    {
        _culture = culture;
        _culture.Collected += OnCollected;
        _pouredReaction = _culture.IsPoured.Subscribe(OnPoredChanged);
        _grownReaction = _culture.IsGrown.Subscribe(OnGrownChanged);
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
            _culture.IsGrown.Dispose();
            _culture.Dispose();
        }

        Object.Destroy(_prefab);

        ActivateVisualState(false);
        _pouredVisuals.SetActive(false);
        
        _pouredReaction.Dispose();
        _grownReaction.Dispose();
    }

    private void OnCollected()
    {
        Performed?.Invoke();
        Collected?.Invoke();
    }

    private void OnPoredChanged(bool value)
    {
        _pouredVisuals.SetActive(value);
        if (value)
        {
            Object.Destroy(_prefab);
            _prefab = Object.Instantiate(_culture.CultureConfig.Step2Prefab, _visualState.transform);
            this.Log($"{_culture.Type} was poured");
            Watered?.Invoke();
        }
    }
    
    private void OnGrownChanged(bool value)
    {
        if (value)
        {
            Object.Destroy(_prefab);
            _prefab = Object.Instantiate(_culture.CultureConfig.Step3Prefab, _visualState.transform);
            this.Log($"{_culture.Type} was grown");
            Grown?.Invoke();
        }
    }
    
    private void ActivateVisualState(bool isActive)
    {
        _visualState.gameObject.SetActive(isActive);
    }
}