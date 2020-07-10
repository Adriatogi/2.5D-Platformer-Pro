using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform _targetA, _targetB;
    [SerializeField]
    private float _speed = 1.0f;

    private bool _switchingDirection = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_switchingDirection == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetB.position, Time.deltaTime * _speed);
        }
        else if (_switchingDirection == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetA.position, Time.deltaTime * _speed);
        }

        if (transform.position == _targetA.position)
        {
            _switchingDirection = false;
        } 
        else if (transform.position == _targetB.position)
        {
            _switchingDirection = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_targetA.position, _targetB.position);
    }
}
