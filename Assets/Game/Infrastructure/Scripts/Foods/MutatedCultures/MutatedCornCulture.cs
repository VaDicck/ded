using UnityEngine;
using Zenject;

public class MutatedCornCulture : MutatedCulture<MutatedCorn.Factory, MutatedCorn>
{
    public MutatedCornCulture(CultureConfig config, PlayerCultures playerCultures,
        DayCycleService dayCycleService, Transform bedTransform, MutatedCornConfig mutatedConfig)
        : base(config, playerCultures, dayCycleService, bedTransform, mutatedConfig) 
    {
        _mutatedConfig = mutatedConfig;
    }

    public override CultureType Type => CultureType.MutatedCorn;
    
    [Inject] private PlayerResources _playerResources;

    private MutatedCornConfig _mutatedConfig;
    
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
        
        ResourceType randomResource = GetRandomResourceType();
        
        _playerResources.Add(randomResource, GetRangeByResource(randomResource));
        _canGiveResources = false;
    }
    
    private ResourceType GetRandomResourceType()
    {
        return (ResourceType)UnityEngine.Random.Range(0, 3);
    }

    private int GetRangeByResource(ResourceType resourceType)
    {
        Range currentRange = new Range();
        switch (resourceType)
        {
            case ResourceType.Metal:
                currentRange = _mutatedConfig.CountMetal;
                break;
            case ResourceType.Scrap:
                currentRange = _mutatedConfig.CountScrap;
                break;
            case ResourceType.Wood:
                currentRange = _mutatedConfig.CountWood; 
                break;
        }
        return UnityEngine.Random.Range(currentRange.Min, currentRange.Max + 1);
    }
}