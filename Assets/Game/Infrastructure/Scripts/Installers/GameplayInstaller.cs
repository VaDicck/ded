using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<InputController>().AsSingle().NonLazy();
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        Container.Bind<DayCycleService>().AsSingle();
    }
}