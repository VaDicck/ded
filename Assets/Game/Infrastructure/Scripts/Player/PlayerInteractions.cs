using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{ 
    [SerializeField] private float _interactionDistance;
    
    private InputController _inputController;
        
    public void Init(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Start()
    {
        _inputController.InteractPressed += Interact;
    }

    private void Interact()
    {
        Debug.Log("Interact");
        IInteractable interactable = FindInteractable();
        
        if (interactable == null) return;
        
        interactable.Interact();
    }
        
    private IInteractable FindInteractable()
    {
        if (!Physics.Raycast(new Ray(transform.position, transform.forward),
                out var hit, _interactionDistance, LayerMask.GetMask("Interactable"))) 
            return null;
        
        if (hit.transform.TryGetComponent(out IInteractable interactable))
        {
            return interactable;
        }


        return null;
    }
}