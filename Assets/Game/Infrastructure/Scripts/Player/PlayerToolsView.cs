using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class PlayerToolsView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private List<SerializedPair<ToolType, string>> _toolTriggerNames;
    [SerializeField] private List<SerializedPair<ToolType, GameObject>> _tools;
    [SerializeField] private string _idleTriggerName;

    private PlayerTools _playerTools;

    [Inject]
    private void Initialize(PlayerTools playerTools)
    {
        _playerTools = playerTools;
    }

    private void Start()
    {
        _playerTools.ToolChanged += OnToolChanged;
    }

    private void OnToolChanged(ToolType? tool)
    {
        DisableAll();
        
        if (tool == null)
        {
            _animator.SetTrigger(_idleTriggerName);
            return;
        }

        try
        {
            string triggerName = _toolTriggerNames.Find(x => x.Key == (ToolType)tool).Value; 
            _tools.Find(x => x.Key == tool).Value.SetActive(true);
            _animator.SetTrigger(triggerName);
        }
        catch
        {
            // ignored
        }

    }

    private void DisableAll()
    {
        foreach (var pair in _tools)
        {
            pair.Value.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _playerTools.ToolChanged -= OnToolChanged;
    }
}
