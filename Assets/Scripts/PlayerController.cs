using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Rigidbody2D rb2d;
    private ContactFilter2D contactFilter;
    private bool grounded;
    private bool _canDoubleJump = false;
    private Vector2 velocity;
    [SerializeField] private float _jumpHeight = 15.0f;
    [SerializeField] private float _speed = 5.0f;
    private const float shellRadius = 0.01f;
    private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];


    [SerializeField] private float gravityModifier = 3.5f;
    private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    private Vector2 targetVelocity;
    private const float minMoveDistance = 0.001f;
    private Vector2 groundNormal;
    [SerializeField] private float minGroundNormalY = .65f;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;

        //groundNormal = new Vector2(0f, 1f); //assumes there's a flat ground somewhere beneath him;

    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;
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

        _animator.SetFloat("HorizontalInput", horizontalInput);
        _animator.SetFloat("Speed", move.sqrMagnitude);
        _animator.SetFloat("VelocityY", velocity.y);
        _animator.SetBool("IsGrounded", grounded);

        targetVelocity = move * _speed;
    }

    private void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false); // for X-axis

        move = Vector2.up * deltaPosition.y;

        Movement(move, true); // for y-axis
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

    private void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;

                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement == true)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);

                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;

            }
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }

}
