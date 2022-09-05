using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Platformer3D_Rigidbody : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    //movement vars
    private Vector2 moveDir = Vector2.zero;
    private float speed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight;
    [Space(10)]
    [Range(0, 1f)] [SerializeField] private float jumpBufferTime;
    [Range(0, 1f)] [SerializeField] private float coyoteTime;
    //jump vars
    private bool grounded;
    private bool canBufferJump;
    private bool canCoyoteJump;

    [Header("Technical Settings")]
    //external components
    [SerializeField] private GroundDetector3D groundDetector;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //ground detection
        if (groundDetector == null) { Debug.LogError("Please assign a 3D ground detector on" + transform.name + "!"); }
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
    }

    //---------------------Movement--------------------------------
    public void SetMoveDir(Vector2 input)
    {
        if (input.magnitude > 1f) { moveDir = input.normalized; }
        else { moveDir = input; }
    }

    private void UpdateSpeed()
    {
        if (moveDir.magnitude > 0.1f) { speed += acceleration; } //accelerate
        else { speed -= deceleration; } //deceleration
        speed = Mathf.Clamp(speed, 0, topSpeed);
    }

    private void Move()
    {
        Vector2 toMove;
        if (moveDir.magnitude > 0.1f) { toMove = moveDir * (speed * Time.deltaTime); } //don't allow stop through moveDir
        else { toMove = new Vector2(rb.velocity.x, rb.velocity.z).normalized * (speed * Time.deltaTime); } //use old direction when there is no direct moveDir
        rb.velocity = new Vector3(toMove.x, rb.velocity.y, toMove.y);
    }

    //------------------------Jumping--------------------------
    public void Jump()
    {
        if (grounded || canCoyoteJump) { AddJumpForce(); } //jump or coyote jump
        else if (!canBufferJump) { //tried jump but not grounded? activate jump buffer
            StartCoroutine(BufferJumpCo());
        }
    }

    private void AddJumpForce()
    {
        rb.AddForce(new Vector2(0, jumpHeight), ForceMode.Impulse);
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
        if (rb.velocity.y <= 0f) { StartCoroutine(CoyoteJumpCo()); } //fell off a platform? allow coyote jump
    }
}
