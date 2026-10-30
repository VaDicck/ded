using Zenject;

public class MutatedCorn : MutatedFood
{
    public class Factory : PlaceholderFactory<MutatedCorn> { }

    public void Close()
    {
        _animator.SetTrigger("Close");
    }

    public void Open()
    {
        _animator.SetTrigger("Open");
    }
}
