using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class CraftTable : MonoBehaviour, IInteractable
{
    public bool IsInteractable => true;
    public bool IsUsable => false;

    private CraftTableService _service;
    
    [Inject]
    private void Initialize(CraftTableService service)
    {
        _service = service;
    }
    
    public UniTask Interact()
    {
        _service.Show();
        
        return UniTask.CompletedTask;
    }
}

