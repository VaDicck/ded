using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CraftTableView : MonoBehaviour
{
    [SerializeField] private CraftTableProductListView _craftTableProductListView;
    [SerializeField] private CraftTableCraftView _craftTableCraftView;
    [SerializeField] private Button _backButton;

    private CraftTableService _service;
    private InputController _inputController;
    
    private AudioService _audioService;
    
    [Inject]
    private void Initialize(CraftTableService service, InputController inputController, AudioService audioService)
    {
        _service = service;
        _inputController = inputController;
        _audioService = audioService;

        _service.ShowPerformed += Show;
    }

    private void Show()
    {
        _inputController.BackPressed += Hide;
        _backButton.onClick.AddListener(Hide);
        _inputController.SwitchToUi();
        Time.timeScale = 0;
        _audioService.GameTimeScale.Value = Time.timeScale;

        gameObject.SetActive(true);
        _craftTableProductListView.Show();
    }

    private void Hide()
    {
        _inputController.BackPressed -= Hide;
        _backButton.onClick.RemoveListener(Hide);
        _inputController.SwitchToGameplay();
        _craftTableProductListView.Hide();
        _craftTableCraftView.Hide();
        Time.timeScale = 1;
        _audioService.GameTimeScale.Value = Time.timeScale;

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