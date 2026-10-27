using UnityEngine;
using Zenject;

public class MutatedCorn : MutatedFood
{
    public class Factory : PlaceholderFactory<MutatedCorn> { }
}
