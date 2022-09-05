using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Platformer3D_CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    //movement vars
    private Vector2 moveDir = Vector2.zero;
    private Vector3 velocity = Vector3.zero;
    private float speed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity = 9.81f;
    [Space(10)]
    [SerializeField] [Range(0f, 1f)] private float jumpBufferTime;
    [SerializeField] [Range(0f, 1f)] private float coyoteTime;
    //jump vars
    private bool grounded;
    private bool canBufferJump;
    private bool canCoyoteJump;

    //external components
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        //horizontal movement
        UpdateSpeed();
        SetMoveVelocity();
        //vertical movement
        ApplyGravity();
        //apply result velocity
        controller.Move(velocity);
        //check if grounded changed
        GroundCheck();
    }

    //---------------------Movement--------------------------------
    public void SetMoveDir(Vector2 input)
    {
        if (input.magnitude > 1f) { input.Normalize(); }
        moveDir = input;
    }

    private void UpdateSpeed()
    {
        if (moveDir.magnitude > 0.1f) { speed += acceleration; } //accelerate
        else { speed -= acceleration; } //decelerate
        speed = Mathf.Clamp(speed, 0, topSpeed);
    }

    private void SetMoveVelocity()
    {
        Vector2 toMove;
        if (moveDir.magnitude > 0.1f) { toMove = moveDir * (speed * Time.deltaTime); } //don't allow stop through moveDir
        else { toMove = new Vector2(velocity.x, velocity.z).normalized * (speed * Time.deltaTime); } //use old direction when there is no direct moveDir
        velocity = new Vector3(toMove.x, velocity.y, toMove.y);
    }

    //------------------------Jumping--------------------------
    private void ApplyGravity()
    {
        if (!grounded) {
            velocity.y -= gravity * Time.deltaTime;
        }
    }
    
    public void Jump()
    {
        if (grounded || canCoyoteJump) { AddJumpForce(); } //normal jump or coyote jump
        else if (!canBufferJump) { 
            StartCoroutine(BufferJumpCo()); //if no jump buffer is active, record failed jump input
        }
    }

    private void AddJumpForce()
    {
        velocity.y = jumpHeight;
    }

    //---------timers-----------
    private IEnumerator BufferJumpCo()
    {
        canBufferJump = true;
        yield return new WaitForSeconds(jumpBufferTime);
        canBufferJump = false;
    }

    private IEnumerator CoyoteJumpCo()
    {
        canCoyoteJump = true;
        yield return new WaitForSeconds(coyoteTime);
        canCoyoteJump = false;
    }

    //----------Ground check----------
    private void GroundCheck()
    {
        if (!grounded && controller.isGrounded) {
            OnTouchGround();
        }
        if (grounded && !controller.isGrounded) {
            OnLeaveGround();
        }
    }

    private void OnTouchGround()
    {
        grounded = true;
        if (canBufferJump) { AddJumpForce(); }
        else if (velocity.y < 0f) { velocity.y = -0.1f; } //reset gravity if falling
    }

    private void OnLeaveGround()
    {
        grounded = false;
        if (velocity.y < 0f) { StartCoroutine(CoyoteJumpCo()); } //fell off platform, allow coyote jump
    }
}
