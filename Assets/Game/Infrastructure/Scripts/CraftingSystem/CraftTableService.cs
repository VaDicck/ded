using System;

public class CraftTableService
{
    public event Action ShowPerformed;
    public event Action HidePerformed;
    
    public void Show() => ShowPerformed?.Invoke();
    
    public void Hide() => HidePerformed?.Invoke();
}