using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Header("Horizontal movement")]
    [SerializeField] private float horTopSpeed;
    [SerializeField] private float horAcceleration;
    [SerializeField] private float horDeceleration;
    //horizontal move vars
    Vector2 horMoveDir;
    float horSpeed;

    [Header("Vertical movement")]
    [SerializeField] private float verTopSpeed;
    [SerializeField] private float verAcceleration;
    [SerializeField] private float verDeceleration;
    //vertical move vars
    float verMoveDir;
    float verSpeed;

    //external components
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //---------read inputs-----------
    public void SetHorMoveDir(Vector2 dir)
    {
        if (dir.magnitude > 1f) { dir.Normalize(); } //force dir in expected bounds
        horMoveDir = dir;
    }

    public void SetVerMoveDir(float dir)
    {
        if (dir > 1f || dir < -1f) { //force dir in expected bounds
            dir = Mathf.Clamp(dir, -1f, 1f);
        }
        verMoveDir = dir;
    }

    //----------main update loop-------------
    private void Update()
    {
        //hor movement
        UpdateHorSpeed();
        MoveHor();
        //ver movement
        UpdateVerSpeed();
        MoveVer();
    }

    //---------------horizontal movement----------------
    private void UpdateHorSpeed()
    {
        if (horMoveDir.magnitude > 0.1f) { horSpeed += horAcceleration; } //accelerate
        else { horSpeed -= horDeceleration; } //deceleration
        horSpeed = Mathf.Clamp(horSpeed, 0, horTopSpeed);
    }

    private void MoveHor()
    {
        Vector2 toMove;
        if (horMoveDir.magnitude > 0.1f) { toMove = horMoveDir * (horSpeed * Time.deltaTime); } //don't allow stop through moveDir
        else { toMove = new Vector2(rb.velocity.x, rb.velocity.z).normalized * (horSpeed * Time.deltaTime); } //use old direction when there is no direct moveDir
        rb.velocity = new Vector3(toMove.x, rb.velocity.y, toMove.y);
    }

    //----------------vertical movement-----------------
    private void UpdateVerSpeed()
    {
        if (Mathf.Abs(verMoveDir) > 0.1f) { verSpeed += verAcceleration; } //accelerate
        else { verSpeed -= verDeceleration; } //deceleration
        verSpeed = Mathf.Clamp(verSpeed, 0, verTopSpeed);
    }

    private void MoveVer()
    {
        float toMove = verSpeed * Time.deltaTime;
        if (Mathf.Abs(verMoveDir) > 0.1f) { toMove *= verMoveDir; } //don't allow stop through moveDir
        else { toMove *= Mathf.Clamp(rb.velocity.y, -1f, 1f); } //use old direction when there is no direct moveDir
        rb.velocity = new Vector3(rb.velocity.x, toMove, rb.velocity.z);
    }
}