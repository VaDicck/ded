using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class HintsView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    [SerializeField] private List<Bed> _beds;
    
    [Header("UnlockBeds")]
    [SerializeField] private string _unlockBeds;
    private int _bedsUnlocked;
    
    [Header("PlowBeds")]
    [SerializeField] private string _plowBeds;
    private int _bedsPlowed;
    
    [Header("Cultures")]
    [SerializeField] private string _cultures;
    private int _plantedCultures;
    
    [Header("Water")]
    [SerializeField] private string _water;
    private int _bedsWatered;
    
    [Header("WaitNight")]
    [SerializeField] private string _waitNight;
    
    [Header("ProtectPlants")]
    [SerializeField] private string _protectPlants;
    
    [Header("Go to bed")]
    [SerializeField] private string _goToBed;
    
    [Header("Get plants")]
    [SerializeField] private string _getPlants;
    private int _plantsCollected;
    
    [Header("GoToQuota")]
    [SerializeField] private string _goToQuota;
    
    [Header("Congrats")]
    [SerializeField] private string _congrats;
    
    [Header("Cycle")]
    [SerializeField] private string _cycleThis;
    
    private event Action Unlocked;
    private event Action Plowed;
    private event Action Planted;
    private event Action Watered;
    private event Action Collected;

    private DayCycleService _dayCycleService;
    private AlienService _alienService;
    private QuotaService _quotaService;
    
    [Inject]
    private void Initialize(DayCycleService dayCycleService, AlienService alienService, QuotaService quotaService)
    {
        _dayCycleService = dayCycleService;
        _alienService = alienService;
        _quotaService = quotaService;
    }
    
    private void OnEnable()
    {
        foreach (Bed bed in _beds)
        {
            bed.Unlocked += () =>
            {
                _bedsUnlocked++;

                if (_bedsUnlocked == 3)
                    Unlocked?.Invoke();
            };
        }
        
        foreach (Bed bed in _beds)
        {
            bed.Plowed += () =>
            {
                _bedsPlowed++;

                if (_bedsPlowed == 3)
                    Plowed?.Invoke();
            };
        }
        
        foreach (Bed bed in _beds)
        {
            bed.Planted += () =>
            {
                _plantedCultures++;

                if (_plantedCultures == 3)
                    Planted?.Invoke();
            };
        }
        
        foreach (Bed bed in _beds)
        {
            bed.Watered += () =>
            {
                _bedsWatered++;

                if (_bedsWatered == 3)
                    Watered?.Invoke();
            };
        }
        
        Unlocked += () =>
        {
            _text.text = _plowBeds;
            Unlocked = null;
        };

        Plowed += () =>
        {
            _text.text = _cultures;
            Plowed = null;
        };

        Planted += () =>
        {
            _text.text = _water;
            Planted = null;
        };

        Watered += () =>
        {
            _text.text = _waitNight;
            Watered = null;
            _dayCycleService.NightStarted += async () =>
            {
                _text.text = _protectPlants;
                await UniTask.WaitUntil(() => _alienService.AliensEmpty);
                _text.text = _goToBed;

                _dayCycleService.DayStarted += () =>
                {
                    _text.text = _getPlants;

                    foreach (var bed in _beds)
                    {
                        bed.Collected += () =>
                        {
                            _plantsCollected++;
                            if (_plantsCollected == 3)
                                Collected?.Invoke();
                        };
                    }
                };
            };
        };

        Collected += () =>
        {
            Collected = null;
            _text.text = _goToQuota;
            _quotaService.CultureAmountChanged += (element, type, count) =>
            {
                _text.text = _cycleThis;
            };
        };

        

        _quotaService.QuotaReached += () =>
        {
            print("Quota reached");
            _text.text = _congrats;
        };
    }

    private void Start()
    {
        _text.text = _unlockBeds;
    }
}