using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float playerSpeed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] float enemyCheckRadius = 0.2f;

    [SerializeField] bool canMove;

    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsEnemy;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform[] jumpCheck;

    float movementInput;

    bool isDead;
    bool isGrounded;

    Animator anim;
    Rigidbody2D rb;

    EnemyController enemy;

    public int score { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("canMove", canMove);
        if (!canMove)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSurroundings();
        CheckInput();
        CheckOverEnemy();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        UpdateAnimations();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
        }
    }

    void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);     
    }

    void Jump()
    {
        if (!isDead)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    private void ApplyMovement()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(playerSpeed, rb.velocity.y);
        }
    }

    private void UpdateAnimations()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDead", isDead);
    }

    private void CheckOverEnemy()
    {
        if (rb.velocity.y <= 0)
        {
            for (int i = 0; i < jumpCheck.Length; i++)
            {
                if (Physics2D.OverlapCircle(jumpCheck[i].position, groundCheckRadius, whatIsEnemy))
                {
                    enemy = Physics2D.OverlapCircle(jumpCheck[i].position, groundCheckRadius, whatIsEnemy).
                                                                   GetComponentInParent<EnemyController>();

                    KillEnemy();

                    return;
                }
            }
        }
    }

    public void Die()
    {
        isDead = true;
        Time.timeScale = 0.1f;
    }

    private void KillEnemy()
    {
        if (!isDead)
        {
            score += enemy.points;
            enemy.TakeDamage();
            Jump();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(jumpCheck[0].position, enemyCheckRadius);
        Gizmos.DrawWireSphere(jumpCheck[1].position, enemyCheckRadius);
        Gizmos.DrawWireSphere(jumpCheck[2].position, enemyCheckRadius);
    }
}
