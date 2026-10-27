using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlantedBedState : IBedState
{
    public event Action Performed;
    public event Action Watered;
    public event Action Grown;
    public event Action Collected;

    public bool IsInteractable => _culture.IsInteract();
    public bool IsUsable => _culture.IsUsable();
    public ToolType UseTool
    {
        get {
            if (_culture == null)
                return ToolType.None;

            return _culture.UseTool();
        }
    }

    public Culture Culture => _culture;
    private Culture _culture;
    
    private readonly GameObject _visualState;
    private readonly GameObject _pouredVisuals;

    public PlantedBedState(GameObject visualState, GameObject pouredVisuals)
    {
        _visualState = visualState;
        _pouredVisuals = pouredVisuals;
    }
    
    public void SetCulture(Culture culture)
    {
        _culture = culture;
        _culture.SetPlantedBedState(this);
        _culture.Collected += OnCollected;
        _culture.Watered += OnPoured;
        _culture.Grown += OnGrown;
        
        _culture.SetBedTransform(_visualState.transform);
    }

    public UniTask Interact()
    {
        _culture.Interact();
        
        return UniTask.CompletedTask;
    }

    public UniTask<bool> Use(ToolType toolType)
    {
        return UniTask.FromResult(_culture.UseTool(toolType));
    }
    
    public void Enter()
    {
        ActivateVisualState(true);
        _pouredVisuals.SetActive(false);
        this.Log($"Entered with culture {_culture}");
    }
    
    public void Exit()
    {
        if (_culture != null)
        {
            _culture.Collected -= OnCollected;
            _culture.Watered -= OnPoured;
            _culture.Grown -= OnGrown;
            _culture.IsGrown.Dispose();
            _culture.Dispose();
        }

        ActivateVisualState(false);
        _pouredVisuals.SetActive(false);
    }

    private void OnCollected()
    {
        Performed?.Invoke();
        Collected?.Invoke();
    }
    
    private void OnPoured(bool value)
    {
        Watered?.Invoke();
        _pouredVisuals.SetActive(value);
    }
    
    private void OnGrown()
    {
        Grown?.Invoke();
    }
    
    private void ActivateVisualState(bool isActive)
    {
        _visualState.gameObject.SetActive(isActive);
    }
}