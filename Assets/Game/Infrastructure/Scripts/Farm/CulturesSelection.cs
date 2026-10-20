using System;
using System.Threading;

public class CulturesSelection
{
    public event Action<Culture> Selected;
    public event Action Shown;
    public event Action Hidden;

    private readonly CarrotCulture.Factory _carrotFactory;
    private readonly TomatoCulture.Factory _tomatoFactory;
    private readonly CornCulture.Factory _cornFactory;
    private readonly PepperCulture.Factory _pepperFactory;

    private readonly InputController _inputController;
    
    public CancellationToken CancellationToken => _cancellationTokenSource.Token;
    
    private CancellationTokenSource _cancellationTokenSource;
    
    public CulturesSelection(InputController inputController, CarrotCulture.Factory carrotFactory, TomatoCulture.Factory tomatoFactory,
        CornCulture.Factory cornFactory, PepperCulture.Factory pepperFactory)
    {
        _inputController = inputController;
        
        _carrotFactory = carrotFactory;
        _tomatoFactory = tomatoFactory;
        _cornFactory = cornFactory;
        _pepperFactory = pepperFactory;
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
    
    public void Select(CultureType cultureType)
    {
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
        }

        Selected?.Invoke(culture);
    }
}