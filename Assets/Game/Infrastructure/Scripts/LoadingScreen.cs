using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    
    public void Show(IEnumerator routine = null)
    {
        gameObject.SetActive(true);
        if(routine != null)
            StartCoroutine(LoadGameSceneRoutine(routine));
    }

    public void ShowAndLoadNext()
    {
        gameObject.SetActive(true);
        StartCoroutine(LoadGameSceneRoutine(LoadNextRoutine()));
    }

    private IEnumerator LoadNextRoutine()
    {
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator LoadGameSceneRoutine(IEnumerator routine)
    {
        yield return routine;
        Hide();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
