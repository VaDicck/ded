using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ToolsRadialMenu : MonoBehaviour
{
    [SerializeField] private List<SerializedPair<ToolsRadialMenuElement, float>> _elementsWithWeights;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private float _deselectedMagnitude;
    
    private InputController _inputController;
    private SerializedPair<ToolsRadialMenuElement, float> _selectedElement;
    private List<ElementRange> _elementRanges;
    
    [Inject]
    private void Initialize(InputController inputController)
    {
        _inputController = inputController;
        _inputController.OpenRadialMenu += Show;
        _inputController.RadialMenuCanceled += Hide;
    }
    
    private void Start()
    {
        InitializeElementRanges();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0.2f;
    }
    
    private void Update()
    {
        Vector2 direction = _inputController.RadialMenuMousePosition - new Vector2(transform.position.x, transform.position.y);
        
        float pixelDistance = GetPixelDistance(direction);
        
        if (pixelDistance < _deselectedMagnitude)
        {
            ClearSelection();
            _selectedElement = null;
            return;
        }
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        angle = (angle + 90 - 180 + 360) % 360;
        
        _selectedElement = GetElementByAngle(angle);
        

        ClearSelection();
        
        if (_selectedElement != null)
        {
            _selectedElement.Key.Select();
        }
    }
    
    private float GetPixelDistance(Vector2 direction)
    {
        return direction.magnitude / _canvas.scaleFactor;
    }
    
    private void ClearSelection()
    {
        foreach (var element in _elementsWithWeights)
        {
            element.Key.Deselect();
        }
    }
    
    private void InitializeElementRanges()
    {
        _elementRanges = new List<ElementRange>();
        float currentStartAngle = 0f;
        
        float totalWeight = _elementsWithWeights.Sum(x => x.Value);
        
        foreach (var element in _elementsWithWeights)
        {
            float elementAngle = (360f * element.Value) / totalWeight;
            
            _elementRanges.Add(new ElementRange
            {
                ElementIndex = _elementsWithWeights.IndexOf(element),
                StartAngle = currentStartAngle,
                EndAngle = currentStartAngle + elementAngle
            });
            
            currentStartAngle += elementAngle;
        }
    }
    
    private SerializedPair<ToolsRadialMenuElement, float> GetElementByAngle(float angle)
    {
        for (int i = 0; i < _elementRanges.Count; i++)
        {
            if (angle >= _elementRanges[i].StartAngle && angle < _elementRanges[i].EndAngle)
            {
                return _elementsWithWeights[_elementRanges[i].ElementIndex];
            }
        }
        
        return null;
    }
    
    private void Hide()
    {
        if (_selectedElement != null)
        {
            _selectedElement.Key.ApplyTool();
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private class ElementRange
    {
        public int ElementIndex;
        public float StartAngle;
        public float EndAngle;
    }

    private void OnDestroy()
    {
        _inputController.RadialMenuCanceled -= Hide;
    }
}