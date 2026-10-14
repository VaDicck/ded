using UnityEngine;
using Zenject;

public class PlayerInteractions : MonoBehaviour
{ 
    [SerializeField] private float _interactionDistance;
    [SerializeField] private PlayerTools _playerTools;
    
    private InputController _inputController;
    private IInteractable _interactable;
    
    [Inject]
    private void Init(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Start()
    {
        _inputController.InteractPressed += Interact;
    }

    private void Update()
    {
        FindInteractable();
    }

    private void Interact()
    {
        if (_interactable == null) return;

        if (_interactable.IsInteractable)
        {
            _interactable.Interact();
            return;
        }

        if (_interactable.IsUsable)
            _interactable.Use(_playerTools.Tool);
    }
        
    private void FindInteractable()
    {
        if (Physics.Raycast(new Ray(transform.position, transform.forward),
                out var hit, _interactionDistance, LayerMask.GetMask("Interactable")))
        {
            if (hit.transform.TryGetComponent<IInteractable>(out var interactable))
            {
                _interactable = interactable;
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
}