using UnityEngine;

public abstract class BaseTool : MonoBehaviour
{
    public ToolType ToolType { get; private set; }
    
    protected BaseTool(ToolType toolType)
    {
        ToolType = toolType;
    }
}
