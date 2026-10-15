using UnityEngine;
using Zenject;

public class CultureConfigsInstaller : MonoInstaller
{
    [SerializeField] private CarrotCultureConfig _carrotConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<CarrotCultureConfig>().FromInstance(_carrotConfig).AsSingle();
    }
}