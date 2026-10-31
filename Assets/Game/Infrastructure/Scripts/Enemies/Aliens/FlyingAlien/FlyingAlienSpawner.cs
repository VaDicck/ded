using Zenject;

public class FlyingAlienSpawner : AlienSpawner<FlyingAlien>
{
    [Inject]
    public void InitFactory(FlyingAlien.Factory alienFactory)
    {
        _alienFactory = alienFactory;
    }

    protected override int GetCountAliens()
    {
        return _alienService.CountFlyingAlienInWave;
    }
}