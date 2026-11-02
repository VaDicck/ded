using System.Collections.Generic;
using R3;
using UnityEngine;
using Zenject;

public class PlayerGrenadesView : MonoBehaviour
{
    [SerializeField] private float _throwForce;
    [SerializeField] private List<SerializedPair<ToolType, BaseGrenade>> _grenadePrefabs;
    [SerializeField] private Animator _animator;
    
    private PlayerGrenades _playerGrenades;
    private InputController _inputController;
    private PlayerTools _playerTools;

    private readonly ReactiveProperty<ToolType?> _grenade = new(null);
    
    [Inject]
    private void Initialize(PlayerGrenades playerGrenades, InputController inputController, PlayerTools playerTools)
    {
        _playerGrenades = playerGrenades;
        _inputController = inputController;
        _playerTools = playerTools;
    }

    private void Start()
    {
        _playerTools.ToolChanged += OnToolChanged;
        _grenade.Subscribe(OnGrenadeChanged);
    }

    private void OnToolChanged(ToolType? tool)
    {
        if (tool == null 
            || _playerTools.Contains((ToolType) tool) == false
            || _playerGrenades.Contains((ToolType) tool) == false)
        {
            _grenade.Value = null;

            return;
        }
        
        _grenade.Value = tool;
    }

    private void OnGrenadeChanged(ToolType? grenade)
    {
        if (grenade == null)
        {
            _inputController.ShootPressed -= OnShootPressed;

            return;
        }

        _inputController.ShootPressed += OnShootPressed;
    }

    private void OnShootPressed()
    {
        if (_grenade.Value == null)
            return;
        
        var prefab = _grenadePrefabs.Find(x => x.Key == (ToolType) _grenade.Value);

        if (prefab == null)
        {
            this.LogError($"Grenade prefab for {_grenade.Value} not found");

            return;
        }

        if (_playerGrenades.Use((ToolType)_grenade.Value))
        {
            var grenade = Instantiate(prefab.Value, transform.position, Quaternion.identity);
        
            var throwVector = transform.forward;
            throwVector.y += 0.1f;
            throwVector.Normalize();
            grenade.Throw(throwVector * _throwForce);
        }
    }
}