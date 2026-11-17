using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using R3;

public class MutagenBath : MonoBehaviour, IInteractable
{
    public event Action CanTakeBioTrash;
    public event Action Interacted;
    public ReadOnlyReactiveProperty<float> CurrentCountBioTrash => _currentCountBioTrash;
    
    [SerializeField] private float _bioTrashPerDay = 2;
    [SerializeField] private GameObject _vfx;
    [SerializeField] private GameObject _bioTrashMinimum;
    [SerializeField] private GameObject _bioTrashMaximum;
    [SerializeField] private GameObject _bioTrashFall;
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _takeMutagenSound;
    
    [Inject] private PlayerResources _playerResources;
    [Inject] private DayCycleService _dayCycleService;
    
    private const float MaxBioTrash = 5;

    private readonly ReactiveProperty<float> _currentCountBioTrash = new();

    private bool _canTake;
    private bool _canInteract; //for Tutorial
    
    public bool IsInteractable => _dayCycleService.IsDay && _canTake && _canInteract;
    public bool IsUsable => false;
    
    public UniTask Interact()
    {
        int amountToGive = Mathf.FloorToInt(_currentCountBioTrash.Value);
        
        _currentCountBioTrash.Value -= amountToGive;
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
        _bioTrashMinimum.SetActive(false);
        _bioTrashMaximum.SetActive(false);
        _bioTrashFall.SetActive(false);
        _dayCycleService.NightEnded += OnNightEnded;
        
        _currentCountBioTrash.Subscribe(OnChangeCountBioTrash);
    }

    [ContextMenu("Interact")]
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
        _currentCountBioTrash.Value = Mathf.Min(_currentCountBioTrash.Value + _bioTrashPerDay, MaxBioTrash);

        if (_currentCountBioTrash.Value < 1 || _canTake)
            return;

        _vfx.SetActive(true);
        CanTakeBioTrash?.Invoke();
        _canTake = true;
    }

    private void OnChangeCountBioTrash(float value)
    {
        switch (value)
        {
            case < 1:
                _bioTrashMinimum.SetActive(false);
                _bioTrashMaximum.SetActive(false);
                _bioTrashFall.SetActive(false);
                return;
            case < 2:
                _bioTrashMinimum.SetActive(true);
                _bioTrashMaximum.SetActive(false);
                _bioTrashFall.SetActive(false);
                return;
            case < MaxBioTrash-1:
                _bioTrashMinimum.SetActive(false);
                _bioTrashMaximum.SetActive(true);
                _bioTrashFall.SetActive(false);
                return;
        }

        _bioTrashMinimum.SetActive(false);
        _bioTrashMaximum.SetActive(true);
        _bioTrashFall.SetActive(true);
    }

    private void OnDestroy()
    {
        _dayCycleService.NightEnded -= OnNightEnded;
    }
}