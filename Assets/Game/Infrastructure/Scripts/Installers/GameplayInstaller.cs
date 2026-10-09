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
            .UnderTransformGroup("FleeingAliens");
        
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        Container.Bind<DayCycleService>().AsSingle();
    }
}