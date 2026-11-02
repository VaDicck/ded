using System;
using UnityEngine;
using Zenject;

public class PlayerInteractions : MonoBehaviour
{ 
    public ToolType UsableTool { get; private set; }
    public bool IsInteractableFound { get; private set; }
    
    [SerializeField] private float _interactionDistance;
    
    [Header("AudioClip")]
    [SerializeField] private AudioClip _pickaxeSound;
    [SerializeField] private AudioClip _rakeSound;
    [SerializeField] private AudioClip _pitchforkSound;
    [SerializeField] private AudioClip _wateringSound;
    
    private InputController _inputController;
    private PlayerTools _playerTools;
    private AudioService _audioService;
    
    private IInteractable _interactable;
    
    [Inject]
    private void Init(InputController inputController, PlayerTools playerTools, AudioService audioService)
    {
        _inputController = inputController;
        _playerTools = playerTools;
        _audioService = audioService;
    }

    private void Start()
    {
        _inputController.InteractPressed += Interact;
    }

    private void Update()
    {
        FindInteractable();
    }

    private async void Interact()
    {
        if (_interactable == null) return;

        if (_interactable.IsInteractable)
        {
            _interactable.Interact();
            return;
        }

        if (_interactable.IsUsable && _playerTools.Tool != null)
        {
            bool resultUse = await _interactable.Use((ToolType) _playerTools.Tool);

            if(!resultUse)
                return;

            PlaySfxTool((ToolType)_playerTools.Tool);
        }
    }
        
    private void FindInteractable()
    {
        UsableTool = ToolType.None;
        IsInteractableFound = false;
        
        if (Physics.Raycast(new Ray(transform.position, transform.forward),
                out var hit, _interactionDistance, LayerMask.GetMask("Interactable")))
        {
            if (hit.transform.TryGetComponent<IInteractable>(out var interactable))
            {
                _interactable = interactable;

                if (_interactable.IsUsable)
                    UsableTool = _interactable.UseTool;
                else if (_interactable.IsInteractable)
                    IsInteractableFound = true;

            }
            else
            {
                _interactable = null;
            }
        }
        else
        {
            _interactable = null;
        }
    }

    private void PlaySfxTool(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Pickaxe:
                _audioService.PlaySfx(_pickaxeSound);
                break;
            case ToolType.Rake:
                _audioService.PlaySfx(_rakeSound);
                break;
            case ToolType.Pitchfork:
                _audioService.PlaySfx(_pitchforkSound);
                break;
            case ToolType.Bailer:
                _audioService.PlaySfx(_wateringSound);
                break;
        }
    }
}