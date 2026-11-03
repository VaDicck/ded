using DG.Tweening;
using UnityEngine;

public class StartFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    
    private void Start()
    {
        var sequence = DOTween.Sequence();
        
        sequence.Append(_canvasGroup.DOFade(1, 2));
        
        var tween = _canvasGroup.DOFade(0, 2);
        tween.OnComplete(() => Destroy(gameObject));
        
        sequence.Append(tween);
        
        sequence.SetUpdate(true);
        
        sequence.Play();
    }
}
