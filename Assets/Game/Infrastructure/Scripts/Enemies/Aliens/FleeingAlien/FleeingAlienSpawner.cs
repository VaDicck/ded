using Zenject;

public class FleeingAlienSpawner : AlienSpawner<FleeingAlien>
{
    [Inject]
    public void InitFactory(FleeingAlien.Factory alienFactory)
    {
        _alienFactory = alienFactory;
    }
    
    protected override int GetCountAliens()
    {
        return _alienService.CountFleeingAlienInWave;
    }
}