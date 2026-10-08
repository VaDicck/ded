using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    public ToolType Tool => _toolType;
    [SerializeField] private ToolType _toolType;
}