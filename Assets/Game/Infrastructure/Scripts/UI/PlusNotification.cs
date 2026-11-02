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
        var tween = _canvasGroup.DOFade(0, 5);
        tween.OnComplete(() => Destroy(gameObject));
        tween.Play();
    }
}