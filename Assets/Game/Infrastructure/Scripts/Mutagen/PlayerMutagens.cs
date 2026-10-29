using System;

public class PlayerMutagens
{
    public event Action<int> Used;
    public int Count => _count;
    
    private int _count;

    public void Add()
    {
        _count++;
    }

    public bool Use()
    {
        if (_count <= 0)
            return false;
        
        _count--;
        
        Used?.Invoke(_count);
        return true;
    }
}