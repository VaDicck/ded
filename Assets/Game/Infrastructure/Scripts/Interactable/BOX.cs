using UnityEngine;

public class BOX : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interact with BOX");
    }

    public bool Use(BaseTool tool)
    {
        throw new System.NotImplementedException();
    }
}
