using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform _targetA, _targetB;
    [SerializeField]
    private List<Transform> _wayPoints = new List<Transform>();
    [SerializeField]
    private float _speed = 1.0f;

    private bool _switchingDirection = false;

    int i = 0;
    bool movingObject = false;
    Vector3 targetPosition;


    // Start is called before the first frame update
    void Start()
    {
        targetPosition = _wayPoints[i].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * _speed);
            movingObject = true;
        }
        else if (transform.position == targetPosition)
        {
            targetPosition = _wayPoints[i].position;
            movingObject = false;
        }

        if (i == _wayPoints.Count)
        {
            i = 0;
        }
        else if (!movingObject)
        {
            i++;
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
