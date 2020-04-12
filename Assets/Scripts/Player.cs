﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
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

    [SerializeField]
    private int _lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

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

        _UIManager.updateCoinsDisplay(_collectedCoins);
        _UIManager.updateLivesDisplay(_lives);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3 (horizontalInput * _speed, 0);
        Vector3 velocity = direction * _speed;

        //Changed cached _yVelocity
        jumping();

        //Flipping Character

        if (horizontalInput < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            _spriteRenderer.flipX = false;
        }


        // Update movement
        velocity.y = _yVelocity;
        _animator.SetFloat("HorizontalInput", horizontalInput);
        _animator.SetFloat("Speed", direction.sqrMagnitude);
        Debug.Log(_yVelocity);
        _animator.SetFloat("VelocityY", velocity.y);
        _characterController.Move(velocity * Time.deltaTime);
        
    }

    private void jumping()
    {
        if (_characterController.isGrounded)
        {
            _yVelocity = 0;
            //Single Jump
            if (Input.GetKey(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            //Double Jump
            if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump)
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = false;
            }

            //Make player fall
            _yVelocity -= _gravity;
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
        _UIManager.updateLivesDisplay(_lives);

        if(_lives < 1)
        {
            SceneManager.LoadScene(0);
        }
    }
}
