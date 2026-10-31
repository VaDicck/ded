using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class BedsForceUnlocker : MonoBehaviour
{
    [SerializeField] private List<Bed> _beds;

    private void Start()
    {
        _beds.ForEach(bed => bed.UnlockForce());
    }
}
