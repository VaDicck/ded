using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class TESTgiveMutagen : MonoBehaviour, IInteractable
{
    [Inject] private PlayerResources _playerResources;
    
    public bool IsInteractable => true;
    public bool IsUsable => false;
    public UniTask Interact()
    {
        _playerResources.Add(ResourceType.BioTrash, 10);
        return UniTask.CompletedTask;
    }
}
