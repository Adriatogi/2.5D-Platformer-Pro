using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Sprite _doorClose;

    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    private void openDoor()
    {
        _spriteRenderer.sprite = _doorClose;
    }

}
