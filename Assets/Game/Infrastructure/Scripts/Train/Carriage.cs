using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Carriage : MonoBehaviour, IInteractable
{
    public event Action QuotaReached;

    public bool QuotaElementReached => _quota.IsEmpty || _quota.IsReached;
    
    public bool IsUsable => false;
    public bool IsInteractable => !_quota.IsReached;
    
    [SerializeField] private Quota.Element _quotaElement;
    [SerializeField] private CarriageMenu _carriageMenu;
    
    private PlayerCultures _playerCultures;
    private QuotaService _quotaService;
    
    private QuotaElement _quota;
    
    [Inject]
    private void Init(PlayerCultures playerCultures, QuotaService quotaService)
    {
        _playerCultures = playerCultures;
        _quotaService = quotaService;
    }

    public bool Show()
    {
        bool hasQuota = _quotaService.CurrentQuota.TryGet(_quotaElement, out _quota);

        if (hasQuota == false)
            return false;
        
        gameObject.SetActive(true);
        _carriageMenu.Initialize(_quota);
        _quota.Reached += OnComplete;
        
        return true;
    }
    
    public UniTask Interact()
    {
        Fold();
        
        return UniTask.CompletedTask;
    }

    private void Fold()
    {
        foreach (var expected in _quota.Expected)
        {
            int countTakeCulture;
            
            int countExpectedCulture = expected.Value;
            
            /*int countExpectedCulture = 
                _quota.Expected.Find(pair => pair.Key == actual.Key).Value;*/
                
            if(_quota.Actual.TryGetValue(expected.Key, out var countActualCulture))
                countTakeCulture = _playerCultures.Take(expected.Key, countExpectedCulture-countActualCulture);
            else
                countTakeCulture = _playerCultures.Take(expected.Key, countExpectedCulture);
            
            _quota.Fill(expected.Key, countTakeCulture);
            
            this.Log($"Quota: {_quotaElement} filled for {countTakeCulture} culture");
        }
    }

    private void OnComplete()
    {
        this.Log($"{_quota.Element} completed");
        QuotaReached?.Invoke();
        // TODO: galochka idi za kassu
    }

    private void OnDisable()
    {
        this.LogError("Disable in Train");
        if (_quota != null)
            _quota.Reached -= OnComplete;
        
        this.LogError("quota has null");
        _quota = null;    
        
        gameObject.SetActive(false);
        _carriageMenu.Dispose();
    }
}