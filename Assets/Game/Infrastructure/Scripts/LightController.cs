using UnityEngine;
using Zenject;

public class LightController : MonoBehaviour
{
    [SerializeField] private GameObject _dayLight;
    [SerializeField] private GameObject _nightLight;
    
    private DayCycleService _dayCycle;

    [Inject]
    private void Init(DayCycleService dayCycle)
    {
        _dayCycle = dayCycle;
    }

    private void Start()
    {
        _dayCycle.DayStarted += ShowDayLight;
        _dayCycle.DayEnded += HideDayLight;
        _dayCycle.NightStarted += ShowNightLight;
        _dayCycle.NightEnded += HideNightLight;
    }
    
    private void ShowDayLight()
    {
        _dayLight.SetActive(true);
    }

    private void ShowNightLight()
    {
        _nightLight.SetActive(true);
    }
    
    private void HideDayLight()
    {
        _dayLight.SetActive(false);
    }

    private void HideNightLight()
    {
        _nightLight.SetActive(false);
    }

    private void OnDestroy()
    {
        _dayCycle.DayStarted -= ShowDayLight;
        _dayCycle.DayEnded -= HideDayLight;
        _dayCycle.NightStarted -= ShowNightLight;
        _dayCycle.NightEnded -= HideNightLight;
    }
}
