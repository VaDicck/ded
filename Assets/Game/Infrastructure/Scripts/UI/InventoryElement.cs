using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _icon;
    
    public void Initialize(Sprite icon, string itemName, int count, int maxCount)
    {
        _icon.sprite = icon;
        _nameText.text = itemName;
        _slider.value = (float) count / maxCount;
        _countText.text = $"{count}/{maxCount}";
    }

    public void Initialize(Sprite icon, string itemName, int count)
    {
        _slider.gameObject.SetActive(false);
        _icon.sprite = icon;
        _nameText.text = itemName;
        _countText.text = $"{count}";
    }
}