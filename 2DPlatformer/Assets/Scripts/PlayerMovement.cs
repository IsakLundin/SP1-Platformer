using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem gravityUpParticles;
    [SerializeField] private ParticleSystem gravityDownParticles;
    private Animator animator;

    public GameObject groundCheck;
    private bool isGrounded;

    public float movementSpeed = 2f;
    private float defaultMovementSpeed;

    //private bool isMoving;
    private float moveDirection = 0f;
    private bool isJumpPressed = false;
    public float jumpForce = 10f;

    private bool isFacingLeft = false;
    private bool gravityTop = false;

    private Vector3 velocity;
    public float smoothTime = 0.2f;

    [SerializeField] private LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        defaultMovementSpeed = movementSpeed;
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");
        /*if(Mathf.Abs(moveDirection) > 0.05)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }*/

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJumpPressed = true;
            animator.SetTrigger("DoJump");
        }

        ChangeGravity();

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(moveDirection));
    }

    private void FixedUpdate()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f, whatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
            }
        }

        Vector3 calculatedMovement = Vector3.zero;

        float verticalVelocity = 0f;

        if (isGrounded == false)
        {
            verticalVelocity = rigidBody2D.velocity.y;
        }

        calculatedMovement.x = movementSpeed * moveDirection;
        Move(calculatedMovement, isJumpPressed);
        isJumpPressed = false;
    }

    private void Move(Vector3 moveDirection, bool isJumpPressed)
    {
        // Only modify X, leave Y alone
        Vector3 newPos = Vector3.SmoothDamp(rigidBody2D.velocity, moveDirection, ref velocity, smoothTime);
        newPos.y = rigidBody2D.velocity.y;
        rigidBody2D.velocity = newPos;

        if(isJumpPressed == true && isGrounded == true)
        {
            if(gravityTop == true)
            {
                rigidBody2D.AddForce(new Vector2(0f, jumpForce * -100f));
            }
            else if(gravityTop == false)
            {
                rigidBody2D.AddForce(new Vector2(0f, jumpForce * 100f));
            }
        }

        if(moveDirection.x > 0f && isFacingLeft == true)
        {
            FlipSpriteDirection();
        }
        else if (moveDirection.x < 0f && isFacingLeft == false)
        {
            FlipSpriteDirection();
        }
    }

    private void FlipSpriteDirection()
    {
        spriteRenderer.flipX = !isFacingLeft;
        isFacingLeft = !isFacingLeft;
    }

    public bool isFalling()
    {
        if(rigidBody2D.velocity.y < -1f)
        {
            return true;
        }
        return false;
    }

    public void ResetMovementSpeed()
    {
        movementSpeed = defaultMovementSpeed;
    }

    public void SetNewMovementSpeed(float multiplyBy)
    {
        movementSpeed *= multiplyBy;
    }

    public void ChangeGravity()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && isGrounded == false)
        {
            rigidBody2D.gravityScale *= -1;
            Rotation();
        }
    }

    private void Rotation()
    {
        if(gravityTop == false)
        {
            transform.eulerAngles = new Vector3(0, 180f, 180f);
            gravityUpParticles.Play();
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
            gravityDownParticles.Play();
        }
        gravityTop = !gravityTop;
    }
}
