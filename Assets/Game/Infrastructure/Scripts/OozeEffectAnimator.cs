using UnityEngine;
using System.Collections;

public class OozeEffectAnimator : MonoBehaviour
{
    [SerializeField] private string _flowProperty = "_Flow"; 

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _material;

    [SerializeField] private float _animationDuration = 1f;

    private Material _materialInstance; 
    private Coroutine _currentAnimation;

    private void Awake()
    {
        _materialInstance = new Material(_material);
        _meshRenderer.material = _materialInstance;
    }

    private void OnEnable()
    {
        StartFlowAnimation(_animationDuration);
    }

    private void StartFlowAnimation(float duration)
    {
        if (_currentAnimation != null)
            StopCoroutine(_currentAnimation);

        _materialInstance.SetFloat(_flowProperty, 0f); 

        _currentAnimation = StartCoroutine(AnimateFlowCoroutine(duration));
    }

    private IEnumerator AnimateFlowCoroutine(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float flowValue = Mathf.Lerp(0f, 1f, t);

            _materialInstance.SetFloat(_flowProperty, flowValue); 

            yield return null;
        }

        _materialInstance.SetFloat(_flowProperty, 1f);
        gameObject.SetActive(false);
    }
}