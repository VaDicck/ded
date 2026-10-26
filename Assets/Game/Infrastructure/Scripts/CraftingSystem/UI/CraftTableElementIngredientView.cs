using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftTableElementIngredientView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _neededAmount;

    public void Initialize(Sprite iconSprite, int amount)
    {
        _icon.sprite = iconSprite;
        _neededAmount.text = amount.ToString();
    }
}