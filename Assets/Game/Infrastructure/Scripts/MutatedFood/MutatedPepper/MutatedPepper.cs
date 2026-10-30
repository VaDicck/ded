using Zenject;

public class MutatedPepper : MutatedFood
{
    public class Factory : PlaceholderFactory<MutatedPepper> { }

}