using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Platformer2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    //movement vars
    private float moveDir;
    private float speed;
    //move states
    [SerializeField] private bool facingLeft;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float riseGrav;
    [SerializeField] private float fallGrav;
    [Space(10)]
    [Range(0, 1f)] [SerializeField] private float jumpBufferTime;
    [Range(0, 1f)] [SerializeField] private float coyoteTime;
    //jump vars
    private bool grounded;
    private bool rising;

    private bool canBufferJump = false;
    private bool canCoyoteJump = false;

    [Header("Technical Settings")]
    //external components
    [SerializeField] private GroundDetector2D groundDetector;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = fallGrav;
        //ground detection
        if (groundDetector == null) { Debug.LogError("Please assign a 2D ground detector on" + transform.name + "!"); }
        else { RegisterDetectorEvents(); }
    }
    private void RegisterDetectorEvents()
    {
        groundDetector.onTouchGround.AddListener(OnTouchGround);
        groundDetector.onLeaveGround.AddListener(OnLeaveGround);
    }

    private void FixedUpdate()
    {
        UpdateSpeed();
        Move();
        JumpUpdate();
    }

    //---------------------Movement--------------------------------
    public void SetMoveDir(Vector2 input) {
        SetMoveDir(input.x);
    }
    
    public void SetMoveDir(float input)
    {
        moveDir = input;
        moveDir = Mathf.Clamp(moveDir, -1, 1); //clamp to expected range
        if (facingLeft && moveDir > 0f) { Flip(); }
        else if (!facingLeft && moveDir < 0f) { Flip(); }
    }

    private void UpdateSpeed()
    {
        if (Mathf.Abs(moveDir) > 0.1f) { speed += acceleration; } //accelerate
        else { speed -= deceleration; } //decelerate
        speed = Mathf.Clamp(speed, 0, topSpeed);
    }

    private void Move()
    {
        float toMove = speed * Time.deltaTime;
        if (Mathf.Abs(moveDir) > 0.1f) { toMove *= moveDir; } //don't allow stop through moveDir
        else if (facingLeft) { toMove *= -1; } //when facing left and not stopped through moveDir, force right direction
        rb.velocity = new Vector2(toMove, rb.velocity.y);
    }

    //-----------update face dir
    private void Flip()
    {
        facingLeft = !facingLeft;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    //------------------------Jumping--------------------------
    public void StartJump()
    {
        if (grounded || canCoyoteJump) { AddJumpForce(); } //normal jump if grounded, else coyote jump
        else if (!canBufferJump) {
            StartCoroutine(BufferJumpCo()); //tried jump but not grounded? activate jump buffer
        }
    }
    public void EndJump()
    {
        ResetJump();
    }

    private void AddJumpForce()
    {
        rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse); //jump
        rb.gravityScale = riseGrav;
        rising = true;
    }

    private void JumpUpdate()
    {
        //grav check
        if (rising && rb.velocity.y < 0f) {
            ResetJump();
        }
    }

    private void ResetJump()
    {
        rb.gravityScale = fallGrav;
        rising = false;
    }

    //-----------timers-----------
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

    //----------detector event responses----------
    private void OnTouchGround()
    {
        grounded = true;
        if (canBufferJump) { AddJumpForce(); }
    }

    private void OnLeaveGround()
    {
        grounded = false;
        if (rb.velocity.y <= 0f) { StartCoroutine(CoyoteJumpCo()); } //fell off platform? allow coyote jump
    }
}