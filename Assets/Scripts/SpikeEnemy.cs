using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEnemy : MonoBehaviour
{
    private DeadZone _deadZone;
    // Start is called before the first frame update
    void Start()
    {
        _deadZone = GetComponent<DeadZone>();
        _deadZone._enemyDeadZone();
    }
}
