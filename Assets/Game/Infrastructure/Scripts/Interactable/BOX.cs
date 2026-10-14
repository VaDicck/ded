using Cysharp.Threading.Tasks;
using UnityEngine;

public class BOX : MonoBehaviour, IInteractable
{
    public bool IsInteractable => true;
    public bool IsUsable => false;

    public UniTask Interact()
    {
        return UniTask.CompletedTask;
    }

    public bool Use(BaseTool tool)
    {
        throw new System.NotImplementedException();
    }
}
