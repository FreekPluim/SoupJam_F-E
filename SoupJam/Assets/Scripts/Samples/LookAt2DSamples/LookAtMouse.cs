using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = LookAt2D.LookAtMouse(transform);
    }
}
