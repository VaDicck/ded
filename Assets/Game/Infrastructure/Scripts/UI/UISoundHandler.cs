using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UISoundHandler : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Audio")]
    [SerializeField] private AudioClip _downSound;   
    [SerializeField] private AudioClip _upSound;  
    [SerializeField] private AudioClip _hoverSound; 
    
    [Inject] private AudioService _audioService;
    
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        TryPlayAudio(_hoverSound);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TryPlayAudio(_downSound);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TryPlayAudio(_upSound);
    }
    
    private void TryPlayAudio(AudioClip clip)
    {
        if(!_button.interactable) return;
        
        _audioService.PlayUiSfx(clip);
    }
}
