using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using Zenject;

[DefaultExecutionOrder(500)]
public class TutorialView : MonoBehaviour
{
    [SerializeField] private List<GameObject> _frames;
    [SerializeField] private List<VideoClip> _videoClips;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private OnlyApplyPopUp _firstFrame;
    [SerializeField] private List<Bed> _beds;
    [SerializeField] private RockingChair _rockingChair;
    [Inject] private AlienService _alienService;
    [SerializeField] private SleepingBed _sleepingBed;
    [Inject] private QuotaService _quotaService;
    [Inject] private PlayerTools _tools;
    [SerializeField] private MutagenBath _bath;
    
    [Inject] private CraftTableService _craftTableService;
    
    private void Start()
    {
        _bath.CantInteract();
        _rockingChair.SetInteractable(false);
        
        _frames[0].SetActive(true);
        
        _firstFrame.Applied += OnFirstFrameApplied;
        
        _beds.ForEach(x => x.Plowed += OnBedPlowed);
        _beds.ForEach(x => x.Planted += OnBedPlanted);
        _beds.ForEach(x => x.Watered += OnBedWatered);
        
        _rockingChair.Interacted += OnChairUsed;
        
        _alienService.WaveOver += OnDefended;
        
        _beds.ForEach(x => x.Grown += OnAwaken);
        
        _beds.ForEach(x => x.Collected += OnPlantGot);

        _quotaService.QuotaReached += OnQuotaReached;
        
        _tools.ToolUnlocked += OnCrafted;
        
        _beds.ForEach(x => x.Unlocked += OnBedsUnlocked);
        
        _bath.Interacted += OnBathInteracted;
    }

    private async void OnFirstFrameApplied()
    {
        await UniTask.WaitForSeconds(1f);
        _frames[1].SetActive(true);
        _videoPlayer.clip = _videoClips[0];
        _videoPlayer.Play();
    }
    
    private void OnBedPlowed()
    {
        _frames[2].SetActive(true);
        _videoPlayer.clip = _videoClips[1];
        _beds.ForEach(x => x.Plowed -= OnBedPlowed);
    }

    private void OnBedPlanted()
    {
        _frames[3].SetActive(true);
        _videoPlayer.clip = _videoClips[2];
        _beds.ForEach(x => x.Planted -= OnBedPlanted);
    }

    private async void OnBedWatered()
    {
        _beds.ForEach(x => x.Watered -= OnBedWatered);
        await UniTask.WaitForSeconds(3f);
        _frames[4].SetActive(true);
        _videoPlayer.clip = _videoClips[3];
        _rockingChair.SetInteractable(true);
    }

    private async void OnChairUsed()
    {
        _rockingChair.Interacted -= OnChairUsed;
        await UniTask.WaitForSeconds(1f);
        _frames[5].SetActive(true);
        _videoPlayer.clip = _videoClips[4];
    }

    private async void OnDefended()
    {
        _alienService.WaveOver -= OnDefended;
        await UniTask.WaitForSeconds(duration: 3f);
        _frames[6].SetActive(true);
        _videoPlayer.clip = _videoClips[5];
    }

    private async void OnAwaken()
    {
        _beds.ForEach(x => x.Grown -= OnAwaken);
        await UniTask.WaitForSeconds(3f);
        _frames[7].SetActive(true);
        _videoPlayer.clip = _videoClips[6];
    }

    private async void OnPlantGot()
    {
        _beds.ForEach(x => x.Collected -= OnPlantGot);
        await UniTask.WaitForSeconds(2f);
        _frames[8].SetActive(true);
        _videoPlayer.clip = _videoClips[7];
    }

    private async void OnQuotaReached()
    {
        _quotaService.QuotaReached -= OnQuotaReached;
        await UniTask.WaitForSeconds(5f);
        _frames[9].SetActive(true);
        _videoPlayer.clip = _videoClips[8];
    }

    private void OnCrafted()
    {
        _craftTableService.Hide();
        _frames[10].SetActive(true);
        _tools.ToolUnlocked -= OnCrafted;
        _videoPlayer.clip = _videoClips[9];
    }

    private async void OnBedsUnlocked()
    {
        _beds.ForEach(x => x.Unlocked -= OnBedsUnlocked);
        await UniTask.WaitForSeconds(3f);
        _frames[11].SetActive(true);
        _videoPlayer.clip = _videoClips[10];
        _bath.CanInteract();
    }

    private void OnBathInteracted()
    {
        _bath.Interacted -= OnBathInteracted;
        Destroy(gameObject);
    }
}