﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3 (horizontalInput * _speed, 0);
        Vector3 velocity = direction * _speed;

        //Changed cached _yVelocity
        if (_characterController.isGrounded)
        {
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

        // Update movement
        velocity.y = _yVelocity;
        _characterController.Move(velocity * Time.deltaTime);
        
    }
}
