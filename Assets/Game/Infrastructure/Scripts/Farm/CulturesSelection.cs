using System;
using System.Collections.Generic;
using System.Threading;

public class CulturesSelection
{
    public event Action<CultureType> CultureOpened;
    public event Action<Culture> Selected;
    public event Action Shown;
    public event Action Hidden;

    private readonly CarrotCulture.Factory _carrotFactory;
    private readonly TomatoCulture.Factory _tomatoFactory;
    private readonly CornCulture.Factory _cornFactory;
    private readonly PepperCulture.Factory _pepperFactory;
    private readonly GarnetCulture.Factory _garnetFactory;
    private readonly PotatoCulture.Factory _potatoFactory;

    private readonly InputController _inputController;
    
    public CancellationToken CancellationToken => _cancellationTokenSource.Token;
    
    private CancellationTokenSource _cancellationTokenSource;
    
    private readonly List<CultureType> _unlockedCultures;
    
    public CulturesSelection(GameConfig config, InputController inputController,
        CarrotCulture.Factory carrotFactory, TomatoCulture.Factory tomatoFactory,
        CornCulture.Factory cornFactory, PepperCulture.Factory pepperFactory,
        GarnetCulture.Factory garnetCultureFactory, PotatoCulture.Factory potatoCultureFactory)
    {
        _unlockedCultures = new(config.UnlockedCultures);
        
        _inputController = inputController;
        
        _carrotFactory = carrotFactory;
        _tomatoFactory = tomatoFactory;
        _cornFactory = cornFactory;
        _pepperFactory = pepperFactory;
        _garnetFactory = garnetCultureFactory;
        _potatoFactory = potatoCultureFactory;
    }

    public void Show()
    {
        _inputController.InteractStopped += Hide;
        _cancellationTokenSource = new CancellationTokenSource();
        Shown?.Invoke();
    }

    public void Hide()
    {
        _inputController.InteractStopped -= Hide;
        Hidden?.Invoke();
        _cancellationTokenSource.Cancel();
    }

    public bool UnlockCulture(CultureType cultureType)
    {
        if (_unlockedCultures.Contains(cultureType))
            return false;
        
        _unlockedCultures.Add(cultureType);
        CultureOpened?.Invoke(cultureType);
        return true;
    }

    public bool IsUnlocked(CultureType culture)
    {
        return  _unlockedCultures.Contains(culture);
    }
    
    public void Select(CultureType cultureType)
    {
        if (_unlockedCultures.Contains(cultureType) == false)
            return;
        
        Culture culture = null;
        
        switch (cultureType)
        {
            case CultureType.Carrot:
                culture = _carrotFactory.Create();
                break;
            case CultureType.Tomato:
                culture = _tomatoFactory.Create();
                break;
            case CultureType.Corn:
                culture = _cornFactory.Create();
                break;
            case CultureType.Pepper:
                culture = _pepperFactory.Create();
                break;
            case CultureType.Garnet:
                culture = _garnetFactory.Create();
                break;
            case CultureType.Potato:
                culture = _potatoFactory.Create();
                break;
            default:
                this.LogWarning("Unknown culture type: " + cultureType);
                break;
        }

        Selected?.Invoke(culture);
    }
}