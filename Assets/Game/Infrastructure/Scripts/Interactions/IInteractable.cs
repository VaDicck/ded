using Cysharp.Threading.Tasks;

public interface IInteractable
{
    bool IsInteractable { get; }
    bool IsUsable { get; }

    ToolType UseTool => ToolType.None;
    
    UniTask Interact();

    UniTask<bool> Use(ToolType tool) => UniTask.FromResult(false);
}
