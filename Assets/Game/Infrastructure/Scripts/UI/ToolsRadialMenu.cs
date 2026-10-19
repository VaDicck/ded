using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ToolsRadialMenu : MonoBehaviour
{
    [SerializeField] private List<Image> _elements;
    
    private InputController _inputController;
    
    [Inject]
    private void Initialize(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Start()
    {
        _inputController.SwitchToUi();
    }
    
    private void Update()
    {
        Vector2 direction = _inputController.MousePosition - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        angle = (angle + 90 + 360) % 360; 
        
        int element = (int) (angle / (360f / _elements.Count));
        print($"{angle} {element}");
        
        _elements.ForEach(x => x.transform.localScale = Vector3.one);
        _elements[element].transform.localScale = Vector3.one * 1.5f;
    }
}
