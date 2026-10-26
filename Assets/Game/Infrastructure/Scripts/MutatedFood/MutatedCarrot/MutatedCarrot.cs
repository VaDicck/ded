using UnityEngine;
using Zenject;

public class MutatedCarrot : MonoBehaviour
{
    public class Factory : PlaceholderFactory<MutatedCarrot> { }
}
