using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardElement : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _count;

    public void Initialize(Sprite icon, int count)
    {
        _icon.sprite = icon;
        if (count > 0)
            _count.text = count.ToString();
        else 
            _count.text = "";
    }
}
