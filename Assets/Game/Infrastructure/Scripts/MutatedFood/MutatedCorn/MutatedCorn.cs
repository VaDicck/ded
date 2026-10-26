using UnityEngine;
using Zenject;

public class MutatedCorn : MonoBehaviour
{
    public class Factory : PlaceholderFactory<MutatedCorn> { }

    [Inject]
    private void Init()
    {
        
    }
}
