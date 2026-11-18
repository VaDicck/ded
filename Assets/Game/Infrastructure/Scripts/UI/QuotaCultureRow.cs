using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuotaCultureRow : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _progressText;

    public void SetData(Sprite icon, int current, int required)
    {
        _icon.sprite = icon;
        _progressText.text = $"{current}/{required}";
    }
}