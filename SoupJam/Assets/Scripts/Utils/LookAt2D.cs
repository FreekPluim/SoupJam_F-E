using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LookAt2D
{
    /// <summary>
    /// rotates target transform to look towards lookAt position
    /// </summary>
    /// <param name="target">transform to be rotated</param>
    /// <param name="lookAt">position to be looked at</param>
    /// <returns>Output rotation</returns>
    public static Quaternion LookAtTransform(Transform target, Vector3 lookAt)
    {
        Vector2 dir = lookAt - target.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public static Quaternion LookAtTransform(Transform target, Transform lookAt)
    {
        return LookAtTransform(target, lookAt.position);
    }

    /// <summary>
    /// Rotates target transform to look towards mouse cursor. If camera is left empty, main camera will be used
    /// </summary>
    /// <param name="target">transform to be rotated</param>
    /// <param name="targetCamera">camera to get cursor position through. If left empty, main camera will be used</param>
    /// <returns>Output rotation</returns>
    public static Quaternion LookAtMouse(Transform target, Camera targetCamera = null)
    {
        if (targetCamera == null) { targetCamera = Camera.main; }
        return LookAtTransform(target, targetCamera.ScreenToWorldPoint(Input.mousePosition));
    }

    /// <summary>
    /// Rotates target tarnsform to look towards UI element. If camera is left empty, main camera will be used
    /// </summary>
    /// <param name="target">transform to be rotated</param>
    /// <param name="lookAt">target UI element to be looked towards</param>
    /// <param name="targetCamera">camera used to determine UI element position, if left empty, main camera will be used</param>
    /// <returns>Output rotation</returns>
    public static Quaternion LookAtRectTransform(Transform target, RectTransform lookAt, Camera targetCamera = null)
    {
        if (targetCamera == null) { targetCamera = Camera.main; }
        return LookAtTransform (target, targetCamera.ScreenToWorldPoint(lookAt.position));
    }
}
