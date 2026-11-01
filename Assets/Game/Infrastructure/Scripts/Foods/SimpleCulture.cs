using Random = System.Random;
using System;
using R3;
using Zenject;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class SimpleCulture : Culture
{
    public Range DropAmount => _config.DropAmount;
    
    private IDisposable _pouredReaction;
    private IDisposable _grownReaction;
    
    protected GameObject _prefab;
    
    [Inject] protected DiContainer _diContainer;
    
    public SimpleCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService) : base(config, playerCultures, dayCycleService)
    {
        _timerDigOut = _config.Health;
    }

    public override bool IsInteract()
    {
        return _isGrown.Value;
    }
    
    public override bool IsUsable()
    {
        return !IsGrown.CurrentValue;
    }
    
    public override void Interact()
    {
        if (_isPoured.Value == false)
            return;

        if (_isGrown.Value == false)
            return;
        
        this.Log($"{Type} collected");
        
        Random random = new();
        
        _count = random.Next(DropAmount.Min, DropAmount.Max);

        _playerCultures.AddCultureValue(this);
        
        _pouredReaction.Dispose();
        _grownReaction.Dispose();
        Object.Destroy(_prefab);
        
        RaiseCollected();
    }
    
    public override bool UseTool(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Bailer:
                _isPoured.Value = true;
                return true;
        }

        return false;
    }
    
    public override ToolType UseTool()
    {
        if (!_isPoured.Value)
            return ToolType.Bailer;
        
        return ToolType.None;
    }
    
    protected override void OnSetTransform()
    {
        _pouredReaction = IsPoured.Subscribe(OnPouredChanged);
        _grownReaction = IsGrown.Subscribe(OnGrownChanged);
        _prefab = Object.Instantiate(CultureConfig.Step1Prefab, _bedTransform);
    }
    
    private void OnPouredChanged(bool value)
    {
        if (!value) return;
        
        Object.Destroy(_prefab);
        _prefab = Object.Instantiate(CultureConfig.Step2Prefab, _bedTransform);
        this.Log($"{Type} was poured");
    }
    
    private void OnGrownChanged(bool value)
    {
        if (!value) return;
        
        Object.Destroy(_prefab);
        _prefab = Object.Instantiate(CultureConfig.Step3Prefab, _bedTransform);
        this.Log($"{Type} was grown");
        RaiseGrown();
    }

    public override void Dispose()
    {
        base.Dispose();

        if (_prefab != null)
            Object.Destroy(_prefab);
    }
}
