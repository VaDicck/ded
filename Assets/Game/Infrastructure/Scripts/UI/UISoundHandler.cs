using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UISoundHandler : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Audio")]
    [SerializeField] private AudioClip _downSound;   
    [SerializeField] public AudioClip _upSound;  
    [SerializeField] public AudioClip _hoverSound; 
    
    [Inject] private AudioService _audioService;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioService.PlaySfx(_hoverSound);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _audioService.PlaySfx(_downSound);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _audioService.PlaySfx(_upSound);
    }
}
