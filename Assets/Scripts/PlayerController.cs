using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _col;
    private LayerMask _walls;
    private LayerMask _floors;

    public float Speed;
    public float JumpPower;
    public bool IsGrounded;
    public bool TouchWallToLeft;
    public bool TouchWallToRight;

    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _col = gameObject.GetComponent<BoxCollider2D>();
        _walls = LayerMask.GetMask("Walls");
        _floors = LayerMask.GetMask("Floors");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var side_movement = Input.GetAxisRaw("Horizontal");
        var jump = Input.GetAxisRaw("Jump");

        GroundedRay();
        LeftWallRay();
        RightWallRay();

        // Horizontal movement
        Vector2 apply_force = new Vector2(side_movement * Speed, 0) - new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(apply_force, ForceMode2D.Impulse);

        // Jump
        // Edit: Moved to a separate function.
        if (jump > 0)
        {
            PlayerJump(jump);
        }

    }

    private void PlayerJump(float j)
    {
        Debug.Log(j);
        //Simple jumping from the ground up
        if (IsGrounded)
        {
            Debug.Log("applying force");
            Vector2 jump_force = new Vector2(0, JumpPower) - new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(jump_force, ForceMode2D.Impulse);
        }
        //Wall jumping off of a wall to the left
        else if (TouchWallToLeft)
        {
            Debug.Log("wall jump off left");
            Vector2 walljump_left = new Vector2(JumpPower * 2, JumpPower * 4 / 3) - new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(walljump_left, ForceMode2D.Impulse);
            Debug.Log(walljump_left);
        }
        //Wall jumping off of a wall to the left
        else if (TouchWallToRight)
        {
            Debug.Log("wall jump off right");
            Vector2 walljump_right = new Vector2(-JumpPower * 2, JumpPower * 4 / 3) - new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(walljump_right, ForceMode2D.Impulse);
            Debug.Log(walljump_right);
        }
        //Jump pressed midair
        else
        {
            Debug.Log("no jump");
        }
    }

    private void GroundedRay()
    {
        // Get leftmost and rightmost positions on player collider
        Vector2 leftbound = transform.position - new Vector3(_col.bounds.extents.x, 0);
        Vector2 rightbound = transform.position + new Vector3(_col.bounds.extents.x, 0);

        // Raycast down from those positions
        bool GroundLeft = Physics2D.Raycast(leftbound, Vector2.down, _col.bounds.extents.y * 1.1f, _floors);
        bool GroundRight = Physics2D.Raycast(rightbound, Vector2.down, _col.bounds.extents.y * 1.1f, _floors);

        // If either of those raycasts hit floor, ground player. Else, unground.
        bool TouchingGround = GroundLeft || GroundRight;        
        IsGrounded = TouchingGround;
    }

    private void LeftWallRay()
    {
        Vector2 downbound = transform.position - new Vector3(0, _col.bounds.extents.y);
        Vector2 upbound = transform.position + new Vector3(0, _col.bounds.extents.y);
        bool LeftDown = Physics2D.Raycast(downbound, Vector2.left, _col.bounds.extents.x * 1.1f, _walls);
        bool LeftMid = Physics2D.Raycast(transform.position, Vector2.left, +_col.bounds.extents.x * 1.1f, _walls);
        bool LeftUp = Physics2D.Raycast(upbound, Vector2.left, _col.bounds.extents.x * 1.1f, _walls);
        bool TouchingLeft = LeftDown || LeftMid || LeftUp;
        
        TouchWallToLeft = TouchingLeft;
    }

    private void RightWallRay()
    {
        Vector2 downbound = transform.position - new Vector3(0, _col.bounds.extents.y);
        Vector2 upbound = transform.position + new Vector3(0, _col.bounds.extents.y);
        bool RightDown = Physics2D.Raycast(downbound, Vector2.right, _col.bounds.extents.x * 1.1f, _walls);
        bool RightMid = Physics2D.Raycast(transform.position, Vector2.right, _col.bounds.extents.x * 1.1f, _walls);
        bool RightUp = Physics2D.Raycast(upbound, Vector2.right, _col.bounds.extents.x * 1.1f, _walls);
        bool TouchingRight = RightDown || RightMid || RightUp;

        TouchWallToRight = TouchingRight;
    }
}
