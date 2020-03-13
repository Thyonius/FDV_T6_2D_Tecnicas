using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 5.0f;
    [SerializeField] float minSpeed = 1.0f;

    Transform destructionPoint;

    float currentSpeed;

    bool canMove;

    Rigidbody2D rb;
    Animator anim;

    PlayerController player;

    public int points { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        destructionPoint = GameObject.FindWithTag("DestructionPoint").GetComponent<Transform>();
        canMove = true;
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        anim.SetBool("isDamaged", false);
        points = 5;
    }

    private void Update()
    {
        CheckIfVisible();
    }

    private void CheckIfVisible()
    {
        if (this.transform.position.x < destructionPoint.position.x)
        {
            this.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (gameObject.activeInHierarchy && canMove)
        {
            rb.velocity = new Vector2(-currentSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canMove = false;
            anim.SetTrigger("hit");
        }
    }
    public void KillPlayer()
    {
        player.Die();
    }

    public void TakeDamage()
    {
        anim.SetTrigger("isDamaged");
    }
}
