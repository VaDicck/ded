using UnityEngine;
using Zenject;

public class CraftTableView : MonoBehaviour
{
    [SerializeField] private CraftTableProductListView _craftTableProductListView;
    [SerializeField] private CraftTableCraftView _craftTableCraftView;

    private CraftTableService _service;
    private InputController _inputController;
    
    [Inject]
    private void Initialize(CraftTableService service, InputController inputController)
    {
        _service = service;
        _inputController = inputController;

        _service.ShowPerformed += Show;
    }

    private void Show()
    {
        _inputController.BackPressed += Hide;
        _inputController.SwitchToUi();
        gameObject.SetActive(true);
        _craftTableProductListView.Show();
    }

    private void Hide()
    {
        _inputController.BackPressed -= Hide;
        _inputController.SwitchToGameplay();
        _craftTableProductListView.Hide();
        _craftTableCraftView.Hide();
        gameObject.SetActive(false);
    }
    
    private void Start()
    {
        _craftTableProductListView.Selected += _craftTableCraftView.Show;
    }

    private void OnDestroy()
    {
        _craftTableProductListView.Selected += _craftTableCraftView.Show;
    }
}