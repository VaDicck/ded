using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField] private List<Material> _materials;
    [SerializeField] private Renderer _meshRenderer;

    private void Start()
    {
        var material = _materials[Random.Range(0, _materials.Count)];
        
        _meshRenderer.material = material;
    }
}
