using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTransform : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    private void Update()
    {
        transform.rotation = LookAt2D.LookAtTransform(transform, targetTransform.position);
    }
}
