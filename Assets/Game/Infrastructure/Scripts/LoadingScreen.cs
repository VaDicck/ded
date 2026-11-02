using System.Collections;
using UnityEngine;

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
