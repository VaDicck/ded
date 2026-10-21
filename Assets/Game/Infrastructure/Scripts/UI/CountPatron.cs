using TMPro;
using R3;
using UnityEngine;

public class CountPatron : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Weapon _shotgun;
    
    private void Start()
    {
        _shotgun.CurrentCountPatron.Subscribe(UpdateCurrentPatron);
        UpdateCurrentPatron(_shotgun.CurrentCountPatron.CurrentValue);
    }

    private void Show()
    {
        UpdateCurrentPatron(_shotgun.CurrentCountPatron.CurrentValue);
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void UpdateCurrentPatron(int CurrentCountPatron)
    {
        _countText.text = $"Patron {CurrentCountPatron}/{_shotgun.MaxCountPatron}";
    }
}
