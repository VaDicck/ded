using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class TimeFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _text;

    private TimeFadeService _fadeService;
    private InputController _inputController;
    
    [Inject]
    private void Initialize(TimeFadeService fadeService, InputController inputController)
    {
        _fadeService = fadeService;
        _inputController = inputController;
    }

    private void Start()
    {
        _canvasGroup.gameObject.SetActive(false);
        _fadeService.Requested += Fade;
    }
    
    private void Fade(string text, Action callback = null)
    {
        _canvasGroup.gameObject.SetActive(true);
        _text.alpha = 0;
        _text.text = text;
        
        var sequence = DOTween.Sequence();
        _inputController.Disable();
        sequence.Append(_canvasGroup.DOFade(1, 1f));
        sequence.Append(_text.DOFade(1, 1f).OnComplete(() =>
        {
            callback?.Invoke();
            _inputController.Enable();
        }));
        sequence.Append(_canvasGroup.DOFade(0, 1f).OnComplete(() => _canvasGroup.gameObject.SetActive(false)));

        sequence.Play();
    }
}

public class TimeFadeService
{
    public event Action<string, Action> Requested;

    public void Fade(string text, Action callback = null)
    {
        Requested?.Invoke(text, callback);
    }
}
