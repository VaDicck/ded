using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameObject _fleeingAlienPrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<InputController>().AsSingle().NonLazy();

        Container.BindFactory<FleeingAlien, FleeingAlien.Factory>()
            .FromComponentInNewPrefab(_fleeingAlienPrefab)
            .UnderTransformGroup("FleeingAliens");
    }
}