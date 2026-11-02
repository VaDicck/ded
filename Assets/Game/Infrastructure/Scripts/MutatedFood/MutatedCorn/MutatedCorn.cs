using UnityEngine;
using Zenject;

public class MutatedCorn : MutatedFood
{
    public class Factory : PlaceholderFactory<MutatedCorn> { }
    
    [SerializeField] private AudioClip _idleSound;
    [SerializeField] private AudioClip _closeSound;
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _readyItemSound;
    [SerializeField] private AudioClip _sadSound;
    
    private void Close()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_closeSound);
        _animator.SetTrigger("Close");
    }

    public void Open()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_openSound);
        _animator.SetTrigger("Open");
    }
    
    public void CanGiveResources()
    {
        _audioSource.clip = _readyItemSound;
        _audioSource.Play();
    }

    public void Sad()
    {
        _audioSource.clip = _sadSound;
        _audioSource.Play();
    }
     
    protected override void OnPour(bool value)
    {
        base.OnPour(value);

        if (value)
        {
            Close();
        }
        else
        {
            _audioSource.clip = _idleSound;
            _audioSource.Play();
        }
    }
}
