using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;

    private LoadingScreen _loadingScreen;

    [Inject]
    private void Initialize(LoadingScreen loadingScreen)
    {
        _loadingScreen = loadingScreen;
    }

    private void Start()
    {
        _quitButton.onClick.AddListener(Application.Quit);
        _playButton.onClick.AddListener(() =>
        {
            _loadingScreen.Show(LoadGameSceneRoutine());
        });
    }

    private IEnumerator LoadGameSceneRoutine()
    {
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
