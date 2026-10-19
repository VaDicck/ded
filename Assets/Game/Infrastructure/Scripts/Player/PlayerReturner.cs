using System;
using UnityEngine;

public class PlayerReturner
{
    public event Action<Vector3> ReturnOnSafe;
    
    private Vector3 _safePosition;
    
    public PlayerReturner(Vector3 safePosition)
    {
        _safePosition = safePosition;
    }

    public void Return()
    {
        ReturnOnSafe?.Invoke(_safePosition);
    }
}
