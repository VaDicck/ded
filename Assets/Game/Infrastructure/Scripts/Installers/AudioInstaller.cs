using UnityEngine;
using Zenject;

public class AudioInstaller : MonoInstaller
{
    [SerializeField] private AudioService _audioServicePrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<AudioService>().FromComponentInNewPrefab(_audioServicePrefab).AsSingle().NonLazy();
    }
}