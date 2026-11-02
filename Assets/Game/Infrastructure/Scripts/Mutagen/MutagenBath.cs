using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MutagenBath : MonoBehaviour, IInteractable
{
    public event Action CanTakeBioTrash;
    
    [SerializeField] private float _bioTrashPerDay = 2;
    
    [Inject] private PlayerResources _playerResources;
    [Inject] private DayCycleService _dayCycleService;

    private float _currentCountBioTrash = 0;

    private bool _canTake;
    
    public bool IsInteractable => _dayCycleService.IsDay && _currentCountBioTrash>=1;
    public bool IsUsable => false;
    
    public UniTask Interact()
    {
        int amountToGive = Mathf.FloorToInt(_currentCountBioTrash);
        
        _currentCountBioTrash -= amountToGive;
        _canTake = false;
        
        _playerResources.Add(ResourceType.BioTrash, amountToGive);
        return UniTask.CompletedTask;
    }

    private void Start()
    {
        _dayCycleService.NightEnded += OnNightEnded;
    }

    private void OnNightEnded()
    {
        _currentCountBioTrash += _bioTrashPerDay;

        if (_currentCountBioTrash < 1 || _canTake)
            return;

        CanTakeBioTrash?.Invoke();
        _canTake = true;
    }

    private void OnDestroy()
    {
        _dayCycleService.NightEnded -= OnNightEnded;
    }
}