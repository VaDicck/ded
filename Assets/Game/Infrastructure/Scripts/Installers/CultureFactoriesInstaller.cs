using Zenject;

public class CultureFactoriesInstaller : Installer<CultureFactoriesInstaller>
{
    
    public override void InstallBindings()
    {
        Container.BindFactory<CarrotCulture, CarrotCulture.Factory>();
        Container.BindFactory<TomatoCulture, TomatoCulture.Factory>();
        Container.BindFactory<CornCulture, CornCulture.Factory>();
        Container.BindFactory<PepperCulture, PepperCulture.Factory>();
        Container.BindFactory<GarnetCulture, GarnetCulture.Factory>();
        Container.BindFactory<PotatoCulture, PotatoCulture.Factory>();
    }
}