using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 5;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 15;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 100;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;

    [SerializeField]
    private float gravityModifier = 2;

    private BoxCollider2D boxCollider;

    public Vector2 velocity;

    private bool _canDoubleJump = false;
    private Animator _animator;

    private float direction = 1;
    private float moveInput;

    private bool jump;
    private bool doubleJump;

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    private bool grounded;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
        moveInput = Input.GetAxis("Horizontal");

        if (moveInput > Mathf.Epsilon)
        {
            direction = 1;
        }
        else if (moveInput < -Mathf.Epsilon)
        {
            direction = -1;
        }


        #region Jumping 

        if (grounded)
        {
            _canDoubleJump = true;
            velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
        }
        else if(!grounded)
        {
            //Double Jump
            if (Input.GetButtonDown("Jump") && _canDoubleJump)
            {
                doubleJump = true;
            }
        }
        #endregion

        _animator.SetFloat("Look X", direction);
        _animator.SetFloat("HorizontalInput", moveInput);
        _animator.SetFloat("Speed", velocity.sqrMagnitude);
        _animator.SetFloat("VelocityY", velocity.y);
        _animator.SetBool("IsGrounded", grounded);
    }

    private void FixedUpdate()
    {
        #region Jumping
        if (jump)
        {
            // Calculate the velocity required to achieve the target jump height.
            velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            //_canDoubleJump = true;
            jump = false;
        }
        else if (doubleJump)
        {
            velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            _canDoubleJump = false;
            doubleJump = false;
        }
        #endregion

        float acceleration = grounded ? walkAcceleration : airAcceleration;
        float deceleration = grounded ? groundDeceleration : 0;

        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }

        velocity.y += Physics2D.gravity.y * gravityModifier * Time.deltaTime;

        transform.Translate(velocity * Time.deltaTime);

        #region grounded and collisions

        grounded = false;

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider.
            if (hit == boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    grounded = true;
                    velocity.y = 0;
                }

                if (Vector2.Angle(colliderDistance.normal, Vector2.down) < 90 && velocity.y > 0)
                {
                    velocity.y = 0;
                }
            }
        }
        #endregion
    }

    public void setVelocity(float x, float y)
    {
        velocity.x = x;
        velocity.y = y;
    }
}