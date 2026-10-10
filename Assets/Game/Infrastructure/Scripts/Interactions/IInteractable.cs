using Cysharp.Threading.Tasks;

public interface IInteractable
{
    bool IsInteractable { get; }
    bool IsUsable { get; }
    
    UniTask Interact();

    UniTask<bool> Use(ToolType tool) => UniTask.FromResult(false);
}
