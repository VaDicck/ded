using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Carriage : MonoBehaviour, IInteractable
{
    [SerializeField] private CarriageMenu _carriageMenu;
    
    private QuotaElement _quotaElement;
    private PlayerCultures _playerCultures;

    public bool IsInteractable => !_quotaElement.IsReached;
    public bool IsUsable => false;
    
    [Inject]
    private void Init(PlayerCultures playerCultures)
    {
        _playerCultures = playerCultures;
    }
    
    public void SetQuotaElement(QuotaElement quotaElement)
    {
        _quotaElement = quotaElement;
        
        _carriageMenu.SetCulture(quotaElement.Expected);

        _quotaElement.Reached += Complete;
        _quotaElement.Added += _carriageMenu.UpdateCultureText;
    }
    
    public UniTask Interact()
    {
        Fold();
        
        return UniTask.CompletedTask;
    }

    private void Fold()
    {
        foreach (var actual in _quotaElement.Actual)
        {
            int countExpectedCulture = 
                _quotaElement.Expected.Find(pair => pair.Key == actual.Key).Value;
            
            int countTakeCulture = 
                _playerCultures.Take(actual.Key, countExpectedCulture-actual.Value);
            
            _quotaElement.Fill(actual.Key, countTakeCulture);
        }
    }

    private void Complete()
    {
        this.Log("Complete "+ _quotaElement.Element.ToString());
    }
}