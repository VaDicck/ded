using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private FleeingAlien _fleeingAlienPrefab;
    [SerializeField] private FlyingAlien _flyingAlienPrefab;
    [SerializeField] private AlienBroken _alienBrokenPrefab;
    [SerializeField] private MutatedChicken _mutatedChickenPrefab;
    [SerializeField] private QuotaStringView _quotaStringViewPrefab;
    [SerializeField] private CountCultureStringView _countCultureStringViewPrefab;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Transform _safePlayerTransform;
    [SerializeField] private Chicken _chickenPrefab;

    public override void InstallBindings()
    {
        Container.Bind<InputController>().AsSingle().NonLazy();

        Container.BindFactory<FleeingAlien, FleeingAlien.Factory>()
            .FromComponentInNewPrefab(_fleeingAlienPrefab.gameObject)
            .UnderTransformGroup("Enemies");
        
        Container.BindFactory<FlyingAlien, FlyingAlien.Factory>()
            .FromComponentInNewPrefab(_flyingAlienPrefab.gameObject)
            .UnderTransformGroup("Enemies");
        
        Container.BindFactory<AlienBroken, AlienBroken.Factory>()
            .FromComponentInNewPrefab(_alienBrokenPrefab.gameObject)
            .UnderTransformGroup("AlienBroken");
        
        Container.BindFactory<QuotaStringView, QuotaStringView.Factory>()
            .FromComponentInNewPrefab(_quotaStringViewPrefab.gameObject);

        Container.BindFactory<Chicken, Chicken.Factory>().
            FromComponentInNewPrefab(_chickenPrefab.gameObject);
        
        Container.BindFactory<MutatedChicken, MutatedChicken.Factory>()
            .FromComponentInNewPrefab(_mutatedChickenPrefab.gameObject);
        
        Container.BindFactory<CountCultureStringView, CountCultureStringView.Factory>()
            .FromComponentInNewPrefab(_countCultureStringViewPrefab.gameObject);

        
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DayCycleService>().AsSingle().NonLazy();
        Container.Bind<BedService>().AsSingle().NonLazy();
        Container.Bind<AlienService>().AsSingle().NonLazy();
        Container.Bind<PlayerCultures>().AsSingle().NonLazy();
        Container.Bind<PlayerResources>().AsSingle().NonLazy();
        Container.Bind<DayService>().AsSingle().WithArguments(0).NonLazy();
        Container.Bind<QuotaService>().AsSingle().WithArguments(0).NonLazy();
        Container.Bind<PlayerReturner>().AsSingle().WithArguments(_safePlayerTransform.position).NonLazy();
        Container.Bind<CulturesSelection>().AsSingle().NonLazy();
        
        Container.Bind<PlayerMutagens>().AsSingle().NonLazy();
        
        Container.Bind<PlayerGrenades>().AsSingle().NonLazy();
        Container.Bind<PlayerTools>().AsSingle().NonLazy();
        
        Container.Bind<CraftTableService>().AsSingle().NonLazy();
        
        Container.Bind<ChickenSpawner>().AsSingle().WithArguments(_safePlayerTransform.position).NonLazy();
        
        foreach (var product in _gameConfig.CraftingTable.Products)
        {
            Container.Inject(product);

            foreach (var ingredient in product.Ingredients)
            {
                Container.Inject(ingredient.Key);
            }
        }
        
        CultureFactoriesInstaller.Install(Container);
    }
}