using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ToolsRadialMenu : MonoBehaviour
{
    [SerializeField] private List<SerializedPair<ToolsRadialMenuElement, float>> _elementsWithWeights;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private float _deselectedMagnitude;
    [SerializeField] private GameObject _toolText;
    [SerializeField] private List<GameObject> _toDisable;
    
    private InputController _inputController;
    private SerializedPair<ToolsRadialMenuElement, float> _selectedElement;
    private List<ElementRange> _elementRanges;
    
    [Inject]
    private void Initialize(InputController inputController)
    {
        _inputController = inputController;
        _inputController.OpenRadialMenu += Show;
        _inputController.RadialMenuCanceled += Hide;
        _inputController.RadialMenuWheelDown += ShowNextElement;
        _inputController.RadialMenuWheelUp += ShowPrevElement;
    }
    
    private void Start()
    {
        InitializeElementRanges();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0.2f;

        _selectedElement = null;
        
        foreach (var pair in _elementsWithWeights)
        {
            pair.Key.Show();
        }

        foreach (var go in _toDisable)
        {
            go.SetActive(false);
        }
    }
    
    public void ShowNextElement()
    {
        if (_selectedElement == null)
            return;
        
        _selectedElement.Key.SwitchNext();
    }
    
    public void ShowPrevElement()
    {
        if (_selectedElement == null)
            return;
        
        _selectedElement.Key.SwitchPrevious();
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
            if (_selectedElement.Key.SetToolText())
                _toolText.SetActive(true);
        }
    }
    
    private float GetPixelDistance(Vector2 direction)
    {
        return direction.magnitude / _canvas.scaleFactor;
    }
    
    private void ClearSelection()
    {
        _toolText.SetActive(false);
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
        _selectedElement = null;
        
        foreach (var go in _toDisable)
        {
            go.SetActive(true);
        }
    }

    private class ElementRange
    {
        public int ElementIndex;
        public float StartAngle;
        public float EndAngle;
    }

    private void OnDestroy()
    {
        _inputController.OpenRadialMenu -= Show;
        _inputController.RadialMenuCanceled -= Hide;
        _inputController.RadialMenuWheelDown -= ShowNextElement;
        _inputController.RadialMenuWheelUp -= ShowPrevElement;
    }
}