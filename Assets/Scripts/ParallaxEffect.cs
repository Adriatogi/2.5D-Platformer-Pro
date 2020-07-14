using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float _length, _startPos;
    [SerializeField]
    private GameObject _cam;
    [SerializeField]
    private float _parallaxEffect;
    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log(_length);
    }
    void Update()
    {
        float temp = (_cam.transform.position.x * (1 - _parallaxEffect));
        float dist = (_cam.transform.position.x * _parallaxEffect);

        transform.position = new Vector3(_startPos + dist, transform.position.y, transform.position.z);

        if (temp > _startPos + _length)
        {
            _startPos += _length;
        }
        else if (temp < _startPos - _length)
        {
            _startPos -= _length;
        }
    }
}
