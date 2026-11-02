using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlusNotification : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _icon;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Initialize(Sprite sprite, int count)
    {
        _countText.text = $"+{count}";
        _icon.sprite = sprite;
    }

    public void Start()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(_canvasGroup.DOFade(1, 3));
        
        var tween = _canvasGroup.DOFade(0, 1);
        tween.OnComplete(() => Destroy(gameObject));

        sequence.Append(tween);

        sequence.Play();
    }
}