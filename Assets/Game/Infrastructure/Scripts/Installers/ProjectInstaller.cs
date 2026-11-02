using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private LoadingScreen _loadingScreen;
    
    public override void InstallBindings()
    {
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();
        Container.Bind<LoadingScreen>().FromComponentInNewPrefab(_loadingScreen).AsSingle().NonLazy();
    }
}
