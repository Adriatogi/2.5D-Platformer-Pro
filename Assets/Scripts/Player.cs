using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private int _collectedCoins = 0;
    private UIManager _UIManager;
    private GameManager _gameManager;
    private PlayerController _playerController;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private int _lives = 3;

    private void OnEnable()
    {
        EventBroker.CoinCollected += collectedCoin;
    }

    private void OnDisable()
    {
        EventBroker.CoinCollected -= collectedCoin;
    }
    //Called before start and update
    private void Awake()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _playerController = GetComponent<PlayerController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_UIManager == null)
        {
            Debug.LogError("UIManager is null");
        }
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null");
        }
        if(_playerController == null)
        {
            Debug.LogError("PlayerController is null");
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        _UIManager.UpdateCoinsDisplay(_collectedCoins);
        _UIManager.updateLivesDisplay(_lives);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //RaycastHit2D hit = Physics2D.Raycast(_rigidbody2D.position + Vector2.up * 0.2f, lookDirection, 0.5f, LayerMask.GetMask("Default"));
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _spriteRenderer.bounds.size, 0);

            foreach (Collider2D hit in hits)
            {
                //TODO Refactor so it only creates IInteractable if there is one
                    Debug.Log("Hit");
                    //Debug.DrawRay(transform.position, lookDirection, Color.red, 1.0f, false);

                    IInteractable interactable = hit.GetComponent<IInteractable>();
                    //if it isnt nul, run the interactable property of interecated 
                    if (interactable != null)
                    {
                        interactable.Interact();
                    }
            }
        }
    }

    public void collectedCoin()
    {
        _collectedCoins++;
        _UIManager.UpdateCoinsDisplay(_collectedCoins);
    }

    public void damage()
    {
        _lives--;
        //Detach in case you die from moving platform
        transform.parent = null;
        _UIManager.updateLivesDisplay(_lives);
        if (_lives == 0)
        {
            _UIManager.activateRestartUI();
            _gameManager.gameOver();
        }
    }

    public int getLives()
    {
        return _lives;
    }

    public void Respawn()
    {
        _playerController.setVelocity(0, 0);
        _playerController.setDirection(true);
    }
}
