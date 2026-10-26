using System;

public class CraftTableService
{
    public event Action ShowPerformed;
    
    public void Show() => ShowPerformed?.Invoke();
}