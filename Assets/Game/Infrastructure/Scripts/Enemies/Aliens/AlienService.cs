using System;
using System.Collections.Generic;

public class AlienService
{
    public event Action AliensOver;
    
    public bool AliensEmpty => _aliens.Count == 0;
    public HashSet<AlienBase> Aliens => _aliens;

    private HashSet<AlienBase> _aliens  = new HashSet<AlienBase>();

    public void AddAlien(AlienBase alien)
    {
        if (_aliens.Add(alien))
            alien.Death += RemoveAlien;
    }

    private void RemoveAlien(AlienBase alien)
    {
        alien.Death -= RemoveAlien;
        _aliens.Remove(alien);
        
        if(_aliens.Count == 0)
            AliensOver?.Invoke();
    }
}
