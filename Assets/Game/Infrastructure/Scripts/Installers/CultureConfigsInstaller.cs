using UnityEngine;
using Zenject;

public class CultureConfigsInstaller : MonoInstaller
{
    [SerializeField] private CarrotCultureConfig _carrotConfig;
    [SerializeField] private TomatoCultureConfig _tomatoConfig;
    [SerializeField] private CornCultureConfig _cornConfig;
    [SerializeField] private PepperCultureConfig _pepperConfig;
    [SerializeField] private GarnetCultureConfig _garnetConfig;
    [SerializeField] private PotatoCultureConfig _potatoConfig;
    
    [SerializeField] private MutatedCarrotConfig _mutatedCarrotConfig;
    [SerializeField] private MutatedCornConfig _mutatedCornConfig;
    [SerializeField] private MutatedPepperConfig _mutatedPepperConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<CarrotCultureConfig>().FromInstance(_carrotConfig).AsSingle();
        Container.Bind<TomatoCultureConfig>().FromInstance(_tomatoConfig).AsSingle();
        Container.Bind<CornCultureConfig>().FromInstance(_cornConfig).AsSingle();
        Container.Bind<PepperCultureConfig>().FromInstance(_pepperConfig).AsSingle();
        Container.Bind<GarnetCultureConfig>().FromInstance(_garnetConfig).AsSingle();
        Container.Bind<PotatoCultureConfig>().FromInstance(_potatoConfig).AsSingle();
        
        Container.Bind<MutatedCarrotConfig>().FromInstance(_mutatedCarrotConfig).AsSingle();
        Container.Bind<MutatedCornConfig>().FromInstance(_mutatedCornConfig).AsSingle();
        Container.Bind<MutatedPepperConfig>().FromInstance(_mutatedPepperConfig).AsSingle();
        
        Container.BindFactory<MutatedCarrot, MutatedCarrot.Factory>()
            .FromComponentInNewPrefab(_mutatedCarrotConfig.MutatedPrefab)
            .UnderTransformGroup("MutatedCultures");

        Container.BindFactory<MutatedCorn, MutatedCorn.Factory>()
            .FromComponentInNewPrefab(_mutatedCornConfig.MutatedPrefab)
            .UnderTransformGroup("MutatedCultures");
        
        Container.BindFactory<MutatedPepper, MutatedPepper.Factory>()
            .FromComponentInNewPrefab(_mutatedPepperConfig.MutatedPrefab)
            .UnderTransformGroup("MutatedCultures");
    }
}