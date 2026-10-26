using UnityEngine;
using Zenject;

public class CultureConfigsInstaller : MonoInstaller
{
    [SerializeField] private CarrotCultureConfig _carrotConfig;
    [SerializeField] private TomatoCultureConfig _tomatoConfig;
    [SerializeField] private CornCultureConfig _cornConfig;
    [SerializeField] private PepperCultureConfig _pepperConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<CarrotCultureConfig>().FromInstance(_carrotConfig).AsSingle();
        Container.Bind<TomatoCultureConfig>().FromInstance(_tomatoConfig).AsSingle();
        Container.Bind<CornCultureConfig>().FromInstance(_cornConfig).AsSingle();
        Container.Bind<PepperCultureConfig>().FromInstance(_pepperConfig).AsSingle();
        
        Container.BindFactory<MutatedCarrot, MutatedCarrot.Factory>()
            .FromComponentInNewPrefab(_carrotConfig.MutatedPrefab.gameObject)
            .UnderTransformGroup("MutatedCultures");

        Container.BindFactory<MutatedCorn, MutatedCorn.Factory>()
            .FromComponentInNewPrefab(_cornConfig.MutatedPrefab.gameObject)
            .UnderTransformGroup("MutatedCultures");
    }
}