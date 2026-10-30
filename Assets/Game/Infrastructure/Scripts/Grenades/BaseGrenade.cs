using System.Collections;
using UnityEngine;

public abstract class BaseGrenade : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody; 
    
    public void Throw(Vector3 direction)
    {
        _rigidbody.AddForce(direction, ForceMode.Impulse);
        
        StartCoroutine(Explode());
    }

    protected abstract IEnumerator Explode();
}