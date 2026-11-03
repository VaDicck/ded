using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MutagenBath : MonoBehaviour, IInteractable
{
    public event Action CanTakeBioTrash;
    public event Action Interacted;
    
    [SerializeField] private float _bioTrashPerDay = 2;
    [SerializeField] private GameObject _vfx;
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _takeMutagenSound;
    
    [Inject] private PlayerResources _playerResources;
    [Inject] private DayCycleService _dayCycleService;

    private float _currentCountBioTrash = 0;

    private bool _canTake;
    private bool _canInteract; //for Tutorial
    
    public bool IsInteractable => _dayCycleService.IsDay && _currentCountBioTrash>=1 && _canInteract;
    public bool IsUsable => false;
    
    public UniTask Interact()
    {
        int amountToGive = Mathf.FloorToInt(_currentCountBioTrash);
        
        _currentCountBioTrash -= amountToGive;
        _canTake = false;
        _vfx.SetActive(false);
        _audioSource.PlayOneShot(_takeMutagenSound);
        
        _playerResources.Add(ResourceType.BioTrash, amountToGive);
        Interacted?.Invoke();
        return UniTask.CompletedTask;
    }

    private void Start()
    {
        _vfx.SetActive(false);
        _dayCycleService.NightEnded += OnNightEnded;
    }

    public void CanInteract()
    {
        _canInteract = true;
    }

    public void CantInteract()
    {
        _canInteract = false;
    }

    private void OnNightEnded()
    {
        _currentCountBioTrash += _bioTrashPerDay;

        if (_currentCountBioTrash < 1 || _canTake)
            return;

        _vfx.SetActive(true);
        CanTakeBioTrash?.Invoke();
        _canTake = true;
    }

    private void OnDestroy()
    {
        _dayCycleService.NightEnded -= OnNightEnded;
    }
}