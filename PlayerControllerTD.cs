using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    WALKING,
    ATTACKING,
    INTERACTING
}

public class PlayerControllerTD : MonoBehaviour
{
    public PlayerState currentPlayerState;
    [SerializeField] float speed = 0.0f;
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] Animator animator = null; // float : speed , vertical , horizontal
    private Vector2 movement = Vector2.zero;

    private void Start()
    {
        currentPlayerState = PlayerState.WALKING;
        animator.SetFloat("horizontal", 0);
        animator.SetFloat("vertical", -1.0f); // makes the player face down

    }

    // Update is called once per frame
    void Update()
    {
        movement = Vector2.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Attack") && currentPlayerState != PlayerState.ATTACKING)
        {
            StartCoroutine("AttackCoroutine");
        }
        else if (currentPlayerState == PlayerState.WALKING)
        {
            UpdateMovement();
        }
    }

    private void FixedUpdate()
    {
        if (currentPlayerState == PlayerState.WALKING)
        {
            movement.Normalize(); // makes diagonals less speedy
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    private void UpdateMovement()
    {
        if (movement != Vector2.zero)
        {
            //animator.SetFloat("speed", movement.sqrMagnitude);
            animator.SetBool("isMoving", true);
            animator.SetFloat("horizontal", movement.x);
            animator.SetFloat("vertical", movement.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        rb.isKinematic = true;
        animator.SetBool("isAttacking", true);
        currentPlayerState = PlayerState.ATTACKING;
        yield return null;
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(0.8f);
        currentPlayerState = PlayerState.WALKING;
        rb.isKinematic = false;
    }

   
}
