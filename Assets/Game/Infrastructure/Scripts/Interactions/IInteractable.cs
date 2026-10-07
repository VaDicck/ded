public interface IInteractable
{
    void Interact();

    bool Use(BaseTool tool) => false;
}
