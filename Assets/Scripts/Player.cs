using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : PhysicsObject
{
    private CharacterController _characterController;

    [SerializeField]
    float _speed = 5.0f;
    [SerializeField]
    private float _gravity = 1.0f;
    [SerializeField]
    private float _jumpHeight = 15.0f;
    private float _yVelocity;
    private bool _canDoubleJump = false;

    private int _collectedCoins = 0;
    private UIManager _UIManager;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private GameManager _gameManager;

    [SerializeField]
    private int _lives = 3;

    //Called before start and update
    private void Awake()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer is null");
        }
        if (_animator == null)
        {
            Debug.LogError("Animator is null");
        }
        if (_UIManager == null)
        {
            Debug.LogError("UIManager is null");
        }
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _UIManager.updateCoinsDisplay(_collectedCoins);
        _UIManager.updateLivesDisplay(_lives);
    }

    protected override void computeVelocity()
    {

        Vector2 move = Vector2.zero;

        float horizontalInput = Input.GetAxis("Horizontal");
        move.x = horizontalInput;

        //Flipping Character

        if (horizontalInput < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            _spriteRenderer.flipX = false;
        }

        jumping();

        // Update movement
        //velocity.y = _yVelocity;
        _animator.SetFloat("HorizontalInput", horizontalInput);
        _animator.SetFloat("Speed", move.sqrMagnitude);
        _animator.SetFloat("VelocityY", velocity.y);
        _animator.SetBool("IsGrounded", grounded);

        targetVelocity = move * _speed;
    }
    private void jumping()
    {
        if (grounded)
        {
            //velocity.y = 0;
            //Single Jump
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = _jumpHeight;
                groundNormal.y = 1;
                groundNormal.x = 0;
                _canDoubleJump = true;
            }
        }
        else
        {
            //Double Jump
            if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump)
            {
                velocity.y = _jumpHeight;
                groundNormal.y = 1;
                groundNormal.x = 0;
                _canDoubleJump = false;
            }
        }
    }
    public void collectedCoin()
    {
        _collectedCoins++;
        _UIManager.updateCoinsDisplay(_collectedCoins);
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

    public void respawn()
    {
        _spriteRenderer.flipX = false;
    }
}
