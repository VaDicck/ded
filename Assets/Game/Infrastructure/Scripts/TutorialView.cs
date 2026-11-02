using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Zenject;

public class TutorialView : MonoBehaviour
{
    [SerializeField] private List<GameObject> _frames;
    [SerializeField] private List<VideoClip> _videoClips;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private List<Bed> _beds;
    [SerializeField] private RockingChair _rockingChair;
    [Inject] private AlienService _alienService;
    [SerializeField] private SleepingBed _sleepingBed;
    [Inject] private QuotaService _quotaService;
    [Inject] private PlayerTools _tools;
    
    private void Start()
    {
        _frames[0].SetActive(true);
        _frames[1].SetActive(true);
        
        _videoPlayer.clip = _videoClips[0];
        _videoPlayer.Play();
        
        _beds.ForEach(x => x.Plowed += OnBedPlowed);
        _beds.ForEach(x => x.Planted += OnBedPlanted);
        _beds.ForEach(x => x.Watered += OnBedWatered);
        
        _rockingChair.Interacted += OnChairUsed;
        
        _alienService.WaveOver += OnDefended;
        
        _beds.ForEach(x => x.Grown += OnAwaken);
        
        _beds.ForEach(x => x.Collected += OnPlantGot);

        _quotaService.QuotaReached += OnQuotaReached;
        
        _tools.ToolUnlocked += OnCrafted;
    }

    private void OnBedPlowed()
    {
        _frames[2].SetActive(false);
        _videoPlayer.clip = _videoClips[1];
        _beds.ForEach(x => x.Plowed -= OnBedPlowed);
    }

    private void OnBedPlanted()
    {
        _frames[3].SetActive(false);
        _videoPlayer.clip = _videoClips[2];
        _beds.ForEach(x => x.Planted -= OnBedPlanted);
    }

    private void OnBedWatered()
    {
        _frames[4].SetActive(false);
        _videoPlayer.clip = _videoClips[3];
        _beds.ForEach(x => x.Watered -= OnBedWatered);
    }

    private void OnChairUsed()
    {
        _frames[5].SetActive(false);
        _videoPlayer.clip = _videoClips[4];
        _rockingChair.Interacted -= OnChairUsed;
    }

    private void OnDefended()
    {
        _frames[6].SetActive(false);
        _videoPlayer.clip = _videoClips[5];
        _alienService.WaveOver -= OnDefended;
    }

    private void OnAwaken()
    {
        _frames[7].SetActive(false);
        _videoPlayer.clip = _videoClips[6];
        _beds.ForEach(x => x.Grown -= OnAwaken);
    }

    private void OnPlantGot()
    {
        _frames[8].SetActive(false);
        _videoPlayer.clip = _videoClips[7];
        _beds.ForEach(x => x.Collected -= OnBedWatered);
    }

    private void OnQuotaReached()
    {
        _frames[9].SetActive(false);
        _videoPlayer.clip = _videoClips[8];
        _quotaService.QuotaReached -= OnQuotaReached;
    }

    private void OnCrafted()
    {
        _frames[10].SetActive(false);
        _tools.ToolUnlocked -= OnCrafted;
        _videoPlayer.clip = _videoClips[9];
        _beds.ForEach(x => x.Unlocked += OnBedsUnlocked);
    }

    private void OnBedsUnlocked()
    {
        _frames[11].SetActive(false);
        _videoPlayer.clip = _videoClips[10];
        _beds.ForEach(x => x.Unlocked -= OnBedsUnlocked);
        Destroy(gameObject);
    }
}