using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftTableElementIngredientView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _neededAmount;
    [SerializeField] private GameObject _tooltip;
    [SerializeField] private TMP_Text _tooltipText;

    public void OnEnable()
    {
        _tooltip.SetActive(false);
    }
    
    public void Initialize(Sprite iconSprite, string itemName, int amount)
    {
        _icon.sprite = iconSprite;
        _neededAmount.text = amount.ToString();
        _tooltipText.text = itemName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltip.SetActive(true);
        _tooltip.transform.position = eventData.position;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _tooltip.transform.position = eventData.position;
    }
}