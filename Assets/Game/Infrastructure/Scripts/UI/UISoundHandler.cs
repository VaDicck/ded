using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UISoundHandler : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Audio")]
    [SerializeField] private AudioClip _downSound;   
    [SerializeField] private AudioClip _upSound;  
    [SerializeField] private AudioClip _hoverSound; 
    
    [Inject] private AudioService _audioService;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioService.PlayUiSfx(_hoverSound);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _audioService.PlayUiSfx(_downSound);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _audioService.PlayUiSfx(_upSound);
    }
}
