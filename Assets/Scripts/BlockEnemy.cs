using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEnemy : MonoBehaviour
{
    [SerializeField]
    private Transform _targetA,_targetB;
    [SerializeField]
    private Transform _targetA2, _targetB2;
    private bool _switchingDirection = false;
    private bool _switchingDirectionY = false;
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private float _floatSpeed = 10;
    Vector3 position;

    private bool _isDead = false;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Left right movement
        if (_switchingDirection == false)
        {
            position.x = Mathf.MoveTowards(transform.position.x, _targetB.position.x, Time.deltaTime * _speed);
        }
        else if (_switchingDirection == true)
        {
            position.x = Mathf.MoveTowards(transform.position.x, _targetA.position.x, Time.deltaTime * _speed);
        }

        if (transform.position.x == _targetA.position.x)
        {
            _switchingDirection = false;
        }
        else if (transform.position.x == _targetB.position.x)
        {
            _switchingDirection = true;
        }

        //Up and Down movement
        if (_switchingDirectionY == false)
        {
            position.y = Mathf.MoveTowards(transform.position.y, _targetB2.position.y, Time.deltaTime * _floatSpeed);
        }
        else if (_switchingDirectionY == true)
        {
            position.y = Mathf.MoveTowards(transform.position.y, _targetA2.position.y, Time.deltaTime * _floatSpeed);
        }

        if (transform.position.y == _targetA2.position.y)
        {
            _switchingDirectionY = false;
        }
        else if (transform.position.y == _targetB2.position.y)
        {
            _switchingDirectionY = true;
        }
        transform.position = position;
    }
}
