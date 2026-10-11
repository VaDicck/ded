using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameObject _fleeingAlienPrefab;
    [SerializeField] private GameConfig _gameConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<InputController>().AsSingle().NonLazy();

        Container.BindFactory<FleeingAlien, FleeingAlien.Factory>()
            .FromComponentInNewPrefab(_fleeingAlienPrefab)
            .UnderTransformGroup("Enemies");
        
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();
        Container.Bind<DayCycleService>().AsSingle().NonLazy();
        Container.Bind<PlayerCultures>().AsSingle().NonLazy();
        
        Container.Bind<DayService>().AsSingle().WithArguments(0).NonLazy();

    }
}