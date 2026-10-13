using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarriageMenu : MonoBehaviour
{
    [SerializeField] private GameObject _textPrefab;
    [SerializeField] private GameObject _panel;
    
    private Dictionary<CultureType, TMP_Text> _culturesText = new Dictionary<CultureType, TMP_Text>();

    public void SetCulture(List<SerializedPair<CultureType, int>> cultures)
    {
        foreach (var pair in cultures)
        {
            GameObject text = Instantiate(_textPrefab, _panel.transform);

            TMP_Text tmPro = text.GetComponent<TMP_Text>();
            
            _culturesText.TryAdd(pair.Key, tmPro);
            
            tmPro.text = pair.Key.ToString()+ " " + pair.Value.ToString();
        }
    }

    public void UpdateCultureText(CultureType cultureType, int count)
    {
        _culturesText[cultureType].text = cultureType.ToString()+ " " + count.ToString();
    }
}