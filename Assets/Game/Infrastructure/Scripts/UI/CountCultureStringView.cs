using TMPro;
using UnityEngine;
using Zenject;

public class CountCultureStringView : MonoBehaviour
{
    [field: SerializeField] private TMP_Text _text;

    private CultureType _type;
    
    public class Factory : PlaceholderFactory<CountCultureStringView>
    {
        public CountCultureStringView Create(Transform parent)
        {   
            var obj = Create();
            
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;

            return obj;
        }
    }
    
    public void Initialize(CultureType type, int actual)
    {
        _type =  type;
        SetText(_type, actual);
    }

    private void SetText(CultureType type, int actual)
    {
        _text.text = type.ToString() + " : " +  actual.ToString();
    }
    
    public void UpdateActual(int actual)
    {
        SetText(_type, actual);
    }
}
