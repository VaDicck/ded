using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private FleeingAlien _fleeingAlienPrefab;
    [SerializeField] private FlyingAlien _flyingAlienPrefab;
    [SerializeField] private QuotaStringView _quotaStringViewPrefab;
    [SerializeField] private CountCultureStringView _countCultureStringViewPrefab;
    [SerializeField] private GameConfig _gameConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<InputController>().AsSingle().NonLazy();

        Container.BindFactory<FleeingAlien, FleeingAlien.Factory>()
            .FromComponentInNewPrefab(_fleeingAlienPrefab.gameObject)
            .UnderTransformGroup("Enemies");
        
        Container.BindFactory<FlyingAlien, FlyingAlien.Factory>()
            .FromComponentInNewPrefab(_flyingAlienPrefab.gameObject)
            .UnderTransformGroup("Enemies");
        
        Container.BindFactory<QuotaStringView, QuotaStringView.Factory>()
            .FromComponentInNewPrefab(_quotaStringViewPrefab.gameObject);
        
        Container.BindFactory<CountCultureStringView, CountCultureStringView.Factory>()
            .FromComponentInNewPrefab(_countCultureStringViewPrefab.gameObject);

        
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();
        Container.Bind<DayCycleService>().AsSingle().NonLazy();
        Container.Bind<BedService>().AsSingle().NonLazy();
        Container.Bind<PlayerCultures>().AsSingle().NonLazy();
        
        Container.Bind<DayService>().AsSingle().WithArguments(0).NonLazy();
        
        Container.Bind<QuotaService>().AsSingle().WithArguments(0, 0).NonLazy();

        CultureFactoriesInstaller.Install(Container);
    }
}