using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Carriage : MonoBehaviour, IInteractable
{
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

    public void Show()
    {
        bool hasQuota = _quotaService.CurrentQuota.TryGet(_quotaElement, out _quota);

        if (hasQuota == false)
            return;

        gameObject.SetActive(true);
        _carriageMenu.Initialize(_quota);
        _quota.Reached += OnComplete;
    }
    
    public UniTask Interact()
    {
        Fold();
        
        return UniTask.CompletedTask;
    }

    private void Fold()
    {
        foreach (var actual in _quota.Actual)
        {
            int countExpectedCulture = 
                _quota.Expected.Find(pair => pair.Key == actual.Key).Value;
            
            int countTakeCulture = 
                _playerCultures.Take(actual.Key, countExpectedCulture-actual.Value);
            
            _quota.Fill(actual.Key, countTakeCulture);
        }
    }

    private void OnComplete()
    {
        // TODO: galochka idi za kassu
        this.Log($"{_quota.Element} completed");
    }

    private void OnDisable()
    {
        _quota.Reached -= OnComplete;
        _quota = null;
        gameObject.SetActive(false);
        _carriageMenu.Dispose();
    }
}