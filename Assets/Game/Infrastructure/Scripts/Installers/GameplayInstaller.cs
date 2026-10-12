using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameObject _fleeingAlienPrefab;
    [SerializeField] private GameObject _flyingAlienPrefab;
    [SerializeField] private QuotaStringView _quotaStringViewPrefab;
    [SerializeField] private GameConfig _gameConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<InputController>().AsSingle().NonLazy();

        Container.BindFactory<FleeingAlien, FleeingAlien.Factory>()
            .FromComponentInNewPrefab(_fleeingAlienPrefab)
            .UnderTransformGroup("Enemies");
        
        Container.BindFactory<FlyingAlien, FlyingAlien.Factory>()
            .FromComponentInNewPrefab(_flyingAlienPrefab)
            .UnderTransformGroup("Enemies");


        Container.BindFactory<QuotaStringView, QuotaStringView.Factory>()
            .FromComponentInNewPrefab(_quotaStringViewPrefab);
        
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();
        Container.Bind<DayCycleService>().AsSingle().NonLazy();
        Container.Bind<PlayerCultures>().AsSingle().NonLazy();
        
        Container.Bind<DayService>().AsSingle().WithArguments(0).NonLazy();

    }
}