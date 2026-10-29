using System;
using UnityEngine;
using R3;

public class DiggingBedFleeingAlienState : IFleeingAlienState
{
    public event Action Performed;

    private Bed _bed;
    private BedService _bedService;
    
    private readonly Animator _animator;
    private readonly Transform _alienTransform;
    
    private bool _hasStartedDigging;
    private Quaternion _targetRotation;

    public DiggingBedFleeingAlienState(BedService bedService, Animator animator, Transform alienTransform)
    {
        _bedService = bedService;
        _animator = animator;
        _alienTransform = alienTransform;

        _bedService.CanAttackBeds.Subscribe(OnCanAttackChanged);
    }
    
    public void SetBed(Bed bed)
    {
        _bed = bed;
    }

    public void StopHitting()
    {
        if (IsPlantedBed(_bed.CurrentState, out PlantedBedState plantedBedState))
            plantedBedState.Culture.StopHitting();
    }
    
    public void Enter()
    {
        _hasStartedDigging = false;

        Vector3 lookPos = _bed.transform.position - _alienTransform.position;
        lookPos.y = 0f;

        if (lookPos.sqrMagnitude > 0.0001f)
        {
            Quaternion baseRotation = Quaternion.LookRotation(lookPos);
        
            _targetRotation = baseRotation * Quaternion.Euler(0, -90f, 0);
        }
        else
        {
            _targetRotation = _alienTransform.rotation;
            _hasStartedDigging = true;
        }
    }

    public void Tick()
    {
        if (!_hasStartedDigging)
        {
            _alienTransform.rotation = Quaternion.Slerp(_alienTransform.rotation,
                _targetRotation, Time.deltaTime * 12f);  

            if (Quaternion.Angle(_alienTransform.rotation, _targetRotation) < 3f)
            {
                _hasStartedDigging = true;
                
                _animator.SetTrigger("Digging");
                
                if (IsPlantedBed(_bed.CurrentState, out PlantedBedState plantedBedState1))
                    plantedBedState1.Culture.StartHitting();
            }
            
            return; 
        }

        if (IsPlantedBed(_bed.CurrentState, out PlantedBedState plantedBedState2))
        {
            plantedBedState2.Culture.Hit(Time.deltaTime);
            return;
        }
    
        Performed?.Invoke();
    }

    private bool IsPlantedBed(IBedState bedState, out PlantedBedState plantedBedState)
    {
        if (bedState is PlantedBedState)
        {
            plantedBedState = (PlantedBedState)bedState;
            return true;
        }

        plantedBedState = null;
        return false;
    }
    
    private void OnCanAttackChanged(bool CanAttack)
    {
        if (!CanAttack)
            Performed?.Invoke();
    }
}