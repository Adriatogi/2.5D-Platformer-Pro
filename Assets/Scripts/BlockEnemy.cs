using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEnemy : MonoBehaviour
{
    [SerializeField]
    private Transform _floatTargetA, _floatTargetB;
    private bool _switchingDirectionY = false;
    [SerializeField]
    private float _floatSpeed = 10;
    private Vector3 _position;

    // Start is called before the first frame update
    void Awake()
    {
        _position = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Up and Down movement
        if (_switchingDirectionY == false)
        {
            _position.y = Mathf.MoveTowards(transform.position.y, _floatTargetB.position.y, Time.deltaTime * _floatSpeed);
        }
        else if (_switchingDirectionY == true)
        {
            _position.y = Mathf.MoveTowards(transform.position.y, _floatTargetA.position.y, Time.deltaTime * _floatSpeed);
        }

        if (transform.position.y == _floatTargetA.position.y)
        {
            _switchingDirectionY = false;
        }
        else if (transform.position.y == _floatTargetB.position.y)
        {
            _switchingDirectionY = true;
        }

        transform.position= new Vector3(transform.position.x, _position.y);
    }
}
