using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ToolsRadialMenu : MonoBehaviour
{
    [SerializeField] private List<SerializedPair<ToolsRadialMenuElement, float>> _elementsWithWeights;
    
    private InputController _inputController;
    private SerializedPair<ToolsRadialMenuElement, float> _selectedElement;
    private List<ElementRange> _elementRanges;
    
    [Inject]
    private void Initialize(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Start()
    {
        _inputController.SwitchToUi();
        InitializeElementRanges();
        PositionElements();

        _inputController.RadialMenuCanceled += SelectCurrentTool;
    }
    
    private void Update()
    {
        Vector2 direction = _inputController.MousePosition - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        angle = (angle + 90 + 360) % 360;

        _selectedElement = GetElementByAngle(angle);
        

        foreach (var element in _elementsWithWeights)
        {
            element.Key.Deselect();
        }
        
        if (_selectedElement != null)
        {
            _selectedElement.Key.Select();
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
    
    private void PositionElements()
    {
        float radius = GetComponent<RectTransform>().rect.width / 2f;
        
        for (int i = 0; i < _elementRanges.Count; i++)
        {
            var range = _elementRanges[i];
            var element = _elementsWithWeights[range.ElementIndex].Key;

            float centerAngle = range.StartAngle + (range.EndAngle - range.StartAngle) / 2f;
            float angleRad = centerAngle * Mathf.Deg2Rad;

            Vector3 position = new Vector3(
                Mathf.Cos(angleRad) * radius,
                Mathf.Sin(angleRad) * radius,
                0
            );
            
            element.transform.localPosition = position;

            element.transform.localRotation = Quaternion.Euler(0, 0, centerAngle - 90);
        }
    }
    
    private void SelectCurrentTool()
    {
        if (_selectedElement != null)
        {
            _selectedElement.Key.ApplyTool();
            gameObject.SetActive(false);
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
        _inputController.RadialMenuCanceled -= SelectCurrentTool;
    }
}