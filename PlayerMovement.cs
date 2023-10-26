using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;

    Rigidbody2D rb2d;
    Collider2D col2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float yVel = rb2d.velocity.y;
        if (Input.GetButtonDown("Jump") && GroundCheck()) {
            yVel = jumpForce;
        }
        rb2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * walkSpeed, yVel);
    }

    private bool GroundCheck() {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, col2d.bounds.size * 0.98f, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit;
    }
}
