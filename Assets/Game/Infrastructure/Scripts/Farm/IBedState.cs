using System;
using Cysharp.Threading.Tasks;

public interface IBedState : IState
{
    event Action Performed;

    UniTask Interact() => UniTask.CompletedTask;

    UniTask<bool> Use(ToolType tool)
    {
        return UniTask.FromResult(false);
    } 
}
