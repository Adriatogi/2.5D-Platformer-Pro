using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Sprite _doorClose;
    private bool isKeyCollected = false;
    private SpriteRenderer _spriteRenderer;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

    }

    private void OnEnable()
    {
        EventBroker.KeyCollected += openDoor;
    }

    private void OnDisable()
    {
        EventBroker.KeyCollected -= openDoor;
    }
    private void openDoor()
    {
        _spriteRenderer.sprite = _doorClose;
        isKeyCollected = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isKeyCollected)
        {
            _gameManager.levelCompleted();
        }
    }
}
