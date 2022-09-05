using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 moveSpeed;
    [SerializeField] private bool moveOnStart = true;

    //states
    private bool moving;

    private void Start()
    {
        if (moveOnStart) StartMove();
    }

    //----------states------------
    public void StartMove()
    {
        moving = true;
    }

    public void StopMove()
    {
        moving = false;
    }

    //----------movement----------
    private void Update()
    {
        if (moving) Move();
    }

    private void Move()
    {
        transform.Translate(moveSpeed * Time.deltaTime);
    }
}
