using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 rotateSpeed;
    [SerializeField] private bool rotateOnStart = true;

    //states
    private bool rotating;

    private void Start()
    {
        if (rotateOnStart) StartRotate();
    }

    //-----------states--------------
    public void StartRotate()
    {
        rotating = true;
    }

    public void StopRotate()
    {
        rotating = false;
    }

    //------------rotation-------------
    private void Update()
    {
        if (rotating) Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.Self);
    }
}
