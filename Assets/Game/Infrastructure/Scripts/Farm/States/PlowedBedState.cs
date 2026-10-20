using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlowedBedState : IBedState
{
    public bool IsInteractable => true;
    public bool IsUsable => false;
    
    public event Action Performed;
    
    public Culture Culture => _culture;
    
    private Culture _culture;
    private Type _cultureType;
    
    private readonly GameObject _visualState;
    private CulturesSelection _culturesSelection;
    
    public PlowedBedState(GameObject visualState, CulturesSelection culturesSelection)
    {
        _visualState = visualState;
        _culturesSelection = culturesSelection;
    }
    
    public async UniTask Interact()
    {
        _culturesSelection.Show();

        _culturesSelection.Selected += OnCultureChanged;
        
        await UniTask.WhenAny(
            UniTask.WaitUntilValueChanged(this, c => _culture),
            UniTask.WaitUntilCanceled(_culturesSelection.CancellationToken)
        );
        
        _culturesSelection.Selected -= OnCultureChanged;
        
        if (_culture != null)
            Performed?.Invoke();
    }

    private void OnCultureChanged(Culture culture)
    {
        _culture = culture;
    }
    
    public void Enter()
    {
        _culture = null;
        ActivateVisualState(true);
    }

    public void Exit()
    {
        ActivateVisualState(false);
    }
    
    private void ActivateVisualState(bool isActive)
    {
        _visualState.gameObject.SetActive(isActive);
    }
}
