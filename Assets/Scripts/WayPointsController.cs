using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointsController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _wayPoints = new List<Transform>();
    [SerializeField]
    private float _speed = 1.5f;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 previousPosition = _wayPoints[0].position;
        int wayPointCount = _wayPoints.Count;
        for (int i = 0; i < wayPointCount; i++)
        {
            Gizmos.DrawLine(previousPosition, _wayPoints[i].position);
            previousPosition = _wayPoints[i].position;
        }
        Gizmos.DrawLine(_wayPoints[wayPointCount - 1].position, _wayPoints[0].position);
    }
}
