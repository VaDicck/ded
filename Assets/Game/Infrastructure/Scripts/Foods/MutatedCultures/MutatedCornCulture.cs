using UnityEngine;
using Zenject;

public class MutatedCornCulture : MutatedCulture<MutatedCorn.Factory, MutatedCorn>
{
    public MutatedCornCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService, Transform bedTransform)
        : base(config, playerCultures, dayCycleService, bedTransform) 
    {
        
    }

    public override CultureType Type => CultureType.MutatedCorn;
    
    [Inject] private PlayerResources _playerResources;

    private bool _canGiveResources;
    
    public override bool IsInteract()
    {
        return _canGiveResources;
    }

    public override void Interact()
    {
        if(!_canGiveResources)
            return;

        GiveResources();
    }
    
    public override ToolType UseTool()
    {
        if(_canGiveResources)
            return ToolType.None;
        
        if (!_isPoured.Value)
            return ToolType.Bailer;
        
        return ToolType.None;
    }
    
    protected override void OnNightStart()
    {
        if (_isPoured.Value)
        {
            _mutatedFood.Close();
        }
        else
        {
            //TODO rasstroiza
        }
    }

    protected override void OnNightEnded()
    {
        if (_isPoured.Value)
            _canGiveResources = true;
        
        _isPoured.Value = false;
    }

    private void GiveResources()
    {
        _mutatedFood.Open();
        _playerResources.Add(ResourceType.Metal, 1);
        _canGiveResources = false;
    }
}