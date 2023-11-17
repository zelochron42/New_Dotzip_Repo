using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;

    private Animator animator;
    private SpriteRenderer SpriteRenderer;
    Rigidbody2D rb2d;
    Collider2D col2d;

    PlayerHealthStatus playerHealth;

    
    private void OnEnable() // reference for the script:
    {
        playerHealth.OnPlayerDeath.AddListener(DestroyPlayer);
    }
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = FindObjectOfType<PlayerHealthStatus>();
    }

    // Update is called once per frame
    void Update()
    {
       Move();
    }
    private void Move ()
    { float yVel = rb2d.velocity.y;
        if (Input.GetButtonDown("Jump") && GroundCheck()) {
            yVel = jumpForce;
        }
        rb2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * walkSpeed, yVel);
        // this calls the animation
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) animator.SetBool("Walkingbool", true);
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) animator.SetBool("Walkingbool", true);
        else animator.SetBool("Walkingbool", false);

        // This flips the sprits when going left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
         SpriteRenderer.flipX = true;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
         SpriteRenderer.flipX = false;
    }

    private bool GroundCheck() {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, col2d.bounds.size * 0.98f, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit;

    }

    private void DestroyPlayer() // reference for the script:
    {
        Destroy(gameObject);
    }
}
