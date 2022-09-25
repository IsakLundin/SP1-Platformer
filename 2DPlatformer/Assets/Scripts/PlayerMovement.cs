using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem gravityUpParticles;
    [SerializeField] private ParticleSystem gravityDownParticles;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip gravitySwitchClip;
    [SerializeField] private AudioClip jumpClip;
    private Animator animator;
    private TrailRenderer tR;

    public GameObject groundCheck;
    private bool isGrounded;

    public float movementSpeed = 2f;
    private float defaultMovementSpeed;

    //private bool isMoving;
    private float moveDirection = 0f;
    private bool isJumpPressed = false;
    public float jumpForce = 10f;

    private bool isFacingLeft = false;
    public bool gravityTop = false;

    private Vector3 velocity;
    public float smoothTime = 0.2f;

    private bool canDash = true;
    private bool isDashing = false;
    private bool isPressingUp = false;
    private bool directionalDash = false;
    [SerializeField] private float dashPower;
    [SerializeField] private float directionalDashPower;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    [SerializeField] private LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        defaultMovementSpeed = movementSpeed;
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        tR = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        
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

        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            if (gravityTop)
            {
                if (Input.GetAxis("Horizontal") != 0 && Input.GetKey(KeyCode.S))
                {
                    directionalDash = true;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    isPressingUp = true;
                }
                StartCoroutine(Dash());
            }
            else
            {
                if (Input.GetAxis("Horizontal") != 0 && Input.GetKey(KeyCode.W))
                {
                    directionalDash = true;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    isPressingUp = true;
                }
                StartCoroutine(Dash());
            }
            
        }
        directionalDash = false;
        isPressingUp = false;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
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
            audioSource.PlayOneShot(jumpClip);
            if (gravityTop == true)
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
        if(Input.GetKeyDown(KeyCode.K) && isGrounded == false)
        {
            rigidBody2D.gravityScale *= -1;
            audioSource.PlayOneShot(gravitySwitchClip);
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
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float gravity = rigidBody2D.gravityScale;
        rigidBody2D.gravityScale = 0f;
        if (isPressingUp)
        {
            if (gravityTop)
                rigidBody2D.velocity = new Vector2(0, transform.localScale.y * dashPower * -1);
            else
                rigidBody2D.velocity = new Vector2(0, transform.localScale.y * dashPower);
        }
        else if (directionalDash)
        {
            if (isFacingLeft)
            {
                if (gravityTop)
                    rigidBody2D.velocity = new Vector2(Mathf.Sqrt(transform.localScale.x * dashPower) * -1 * directionalDashPower, Mathf.Sqrt(transform.localScale.y * dashPower) * -1 * directionalDashPower);
                else
                    rigidBody2D.velocity = new Vector2(Mathf.Sqrt(transform.localScale.x * dashPower) * -1 * directionalDashPower, Mathf.Sqrt(transform.localScale.y * dashPower) * directionalDashPower);
            }
            else
            {
                if (gravityTop)
                    rigidBody2D.velocity = new Vector2(Mathf.Sqrt(transform.localScale.x * dashPower) * directionalDashPower, Mathf.Sqrt(transform.localScale.y * dashPower) * -1 * directionalDashPower);
                else
                    rigidBody2D.velocity = new Vector2(Mathf.Sqrt(transform.localScale.x * dashPower) * directionalDashPower, Mathf.Sqrt(transform.localScale.y * dashPower) * directionalDashPower);
            }     
        }
        else if (isFacingLeft)
        {
            rigidBody2D.velocity = new Vector2(transform.localScale.x * dashPower * -1, 0);
        }
        else
        {
            rigidBody2D.velocity = new Vector2(transform.localScale.x * dashPower, 0);
        }
        audioSource.PlayOneShot(dashClip);
        tR.emitting = true;
        yield return new WaitForSeconds(dashTime);
        rigidBody2D.gravityScale = gravity;
        isDashing = false;
        tR.emitting = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
