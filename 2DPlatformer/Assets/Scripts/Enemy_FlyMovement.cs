using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FlyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isAlive = true;
    private bool isFacingRight;

    public Transform target;
    [SerializeField] private Transform startPos;
    Vector2 moveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startPos = GetComponentInChildren<Enemy_FlyTargetRange>().startPos;
    }

    void Update()
    {
        anim.SetBool("IsAlive", isAlive);
        

        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }
        if (isFacingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
        }
        if (isAlive)
            rb.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            if (target)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                if (target.position.x - transform.position.x < 0)
                    isFacingRight = false;
                else
                    isFacingRight = true;

                if (target == GetComponentInChildren<Enemy_FlyTargetRange>().startPos && transform.position == target.position)
                    isFacingRight = false;
            }
        }
    }

    public void KillMe()
    {
        rb.gravityScale = 1;
        isAlive = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Vector2 killForce = new Vector2(moveDirection.x, 4);
        rb.AddForce(moveDirection, ForceMode2D.Impulse);
    }
}
