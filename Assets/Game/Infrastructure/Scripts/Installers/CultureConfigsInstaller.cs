using UnityEngine;
using Zenject;

public class CultureConfigsInstaller : MonoInstaller
{
    [SerializeField] private CultureConfig _carrotConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<CultureConfig>().FromInstance(_carrotConfig).AsSingle();
    }
}