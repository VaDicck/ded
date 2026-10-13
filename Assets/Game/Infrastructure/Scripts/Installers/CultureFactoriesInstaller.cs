using Zenject;

public class CultureFactoriesInstaller : Installer<CultureFactoriesInstaller>
{
    
    public override void InstallBindings()
    {
        Container.BindFactory<CarrotCulture, CarrotCulture.Factory>();
    }
}