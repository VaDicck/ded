using UnityEngine;
using Zenject;

public class AudioInstaller : MonoInstaller
{
    [SerializeField] private AudioConfig _audioConfig;

    public override void InstallBindings()
    {
        Container.BindInstance(_audioConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<AudioService>().AsSingle().NonLazy();
    }
}