using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Carriage : MonoBehaviour, IInteractable
{
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

        _quotaElement.Reached += Complete;
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
            int countActualCulture = _playerCultures.Take(actual.Key, actual.Value);
            
            _quotaElement.Fill(actual.Key, countActualCulture);
        }
    }

    private void Complete()
    {
        Debug.Log("Complete "+ _quotaElement.Element.ToString());
    }
}