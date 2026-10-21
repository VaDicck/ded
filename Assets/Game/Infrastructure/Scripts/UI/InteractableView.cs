using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableView : MonoBehaviour
{
    [SerializeField] private PlayerInteractions _playerInteractions;
    [SerializeField] private Image _interactableImage;

    [SerializeField] private Sprite _crosshair;
    [SerializeField] private Sprite _handInteractableSprite;
    [SerializeField] private List<SerializedPair<ToolType, Sprite>> _toolSprites;
    
    private void Update()
    {
        _interactableImage.sprite = null;
        
        if (_playerInteractions.IsInteractableFound)
            _interactableImage.sprite = _handInteractableSprite;
        else if (_playerInteractions.UsableTool != ToolType.None)
            _interactableImage.sprite = _toolSprites.Find(x => x.Key == _playerInteractions.UsableTool).Value;
    }
}