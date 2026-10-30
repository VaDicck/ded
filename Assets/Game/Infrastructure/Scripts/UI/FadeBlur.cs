using System;
using System.Collections;
using UnityEngine;

public class FadeBlur : MonoBehaviour
{
    private static readonly int Blur = Shader.PropertyToID("_Blur");
    [SerializeField] private Material _material;
    [SerializeField] private float _blur;
    [SerializeField] private float _speed;

    private float _value;
    
    private void OnEnable()
    {
        _value = 0;
    }

    private void Update()
    {
        _material.SetFloat(Blur, Mathf.Min(_value, _blur));
        _value += Time.unscaledDeltaTime * _speed;
    }
}
