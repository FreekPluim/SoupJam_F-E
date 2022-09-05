using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownController : MonoBehaviour
{
    [System.Serializable] //mode
    private enum MoveMode {
        SetPosition, SetVelocity, UseForce
    }

    [Header("Settings")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float acceleration;

    [Header("Technical")]
    [Tooltip("Sets the way movement is handled" +
        "\nSet Position: Directly sets the position of the object." +
        "\nSet Velocity: Directly sets the velocity of the object." +
        "\nUse Force: Uses force to move the object.")]
    [SerializeField] private MoveMode mode;

    //vars
    private Vector2 moveDir = Vector2.zero;
    private float speed;

    //external components
    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void SetMoveDir(Vector2 input)
    {
        if (input.magnitude > 1f) { moveDir = input.normalized; }
        else { moveDir = input; }
    }

    private void FixedUpdate()
    {
        UpdateSpeed();
        if (moveDir != Vector2.zero) {
            Move();
        }
    }

    private void UpdateSpeed()
    {
        if (moveDir.magnitude > 0.1f) { speed += acceleration; } //accelerate
        else { speed -= acceleration; } //fix for akward 'hitching' when key is briefly released
        speed = Mathf.Clamp(speed, 0, topSpeed);
    }

    private void Move()
    {
        Vector2 toMove = moveDir * (speed * Time.deltaTime);
        switch (mode) {
            case MoveMode.SetPosition:
                rigidBody.position += toMove;
                break;

            case MoveMode.SetVelocity:
                rigidBody.velocity = toMove;
                break;

            case MoveMode.UseForce:
                rigidBody.AddForce(toMove);
                break;
        }
    }
}