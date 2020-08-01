using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private Transform _fadeTarget;
    [SerializeField]
    private float _floatSpeed = 2.0f;
    private Vector3 _position;

    private bool _collected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_collected )
        {
            ICollectable collectable = GetComponent<ICollectable>();
            if (collectable != null)
            {
                collectable.Collected();
                _collected = true;
            }

            StartCoroutine(FadeOut());

        }
    }

    private IEnumerator FadeOut()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        float _progress = 0.0f;
        _position = transform.position;

        while (_progress < 1)
        {
            Color _tmpColor = GetComponent<SpriteRenderer>().color;

            GetComponent<SpriteRenderer>().color = new Color(_tmpColor.r, _tmpColor.g, _tmpColor.b, Mathf.Lerp(tmp.a, 0, _progress)); //startAlpha = 0 <-- value is in tmp.a
            _position.y = Mathf.MoveTowards(_position.y, _fadeTarget.position.y, Time.deltaTime * _floatSpeed);
            _progress += Time.deltaTime * 1.5f;

            transform.position = _position;
            if (_tmpColor.a <= 0.030f)
            {
                Destroy(transform.parent.gameObject);
            }

            yield return null;
        }
    }
}
