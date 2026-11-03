using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _chickenButton;
    [SerializeField] private Button _ufoButton1;
    [SerializeField] private Button _ufoButton2;
    [SerializeField] private Button _ufoButton3;
    [SerializeField] private Animator _chickenAnimator;
    [SerializeField] private Rigidbody _rbUfo1;
    [SerializeField] private Rigidbody _rbUfo2;
    [SerializeField] private Rigidbody _rbUfo3;
    [SerializeField] private AudioClip _audioClip;

    private LoadingScreen _loadingScreen;
    private AudioService _audioService;
    
    private bool _dieChicken;

    [Inject]
    private void Initialize(LoadingScreen loadingScreen, AudioService audioService)
    {
        _loadingScreen = loadingScreen;
        _audioService = audioService;
            
        _chickenButton.onClick.AddListener(DieChicken);
        _ufoButton1.onClick.AddListener(Ufo1Fall);
        _ufoButton2.onClick.AddListener(Ufo2Fall);
        _ufoButton3.onClick.AddListener(Ufo3Fall);
    }

    private void Start()
    {
        _audioService.PlayMusic(_audioClip);
        
        _quitButton.onClick.AddListener(Application.Quit);
        _playButton.onClick.AddListener(() =>
        {
            _loadingScreen.ShowAndLoadNext();
        });
    }

    private void DieChicken()
    {
        if(_dieChicken)  return;

        _dieChicken = true;
        _chickenAnimator.SetTrigger("Die");
    }
    
    private void Ufo1Fall()
    {
        _rbUfo1.useGravity = true;
    }
    
    private void Ufo2Fall()
    {
        _rbUfo2.useGravity = true;
    }
    
    private void Ufo3Fall()
    {
        _rbUfo3.useGravity = true;
    }

    private IEnumerator LoadGameSceneRoutine()
    {
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnDestroy()
    {
        _chickenButton.onClick.RemoveListener(DieChicken);
        _ufoButton1.onClick.RemoveListener(Ufo1Fall);
        _ufoButton2.onClick.RemoveListener(Ufo2Fall);
        _ufoButton3.onClick.RemoveListener(Ufo3Fall);
    }
}
