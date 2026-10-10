using System;
using Cysharp.Threading.Tasks;

public interface IBedState : IState
{
    bool IsInteractable { get; }
    bool IsUsable { get; }

    event Action Performed;

    UniTask Interact() => UniTask.CompletedTask;

    UniTask<bool> Use(ToolType tool)
    {
        return UniTask.FromResult(false);
    } 
}
