using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundDetector2D : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onTouchGround;
    public UnityEvent onLeaveGround;

    [Header("Settings")]
    [SerializeField] private List<string> ignoreTags;

    private List<Transform> trackedGrounds;

    private void Start() {
        trackedGrounds = new();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ValidGroundCheck(collision.transform)) {
            if (trackedGrounds.Count <= 0) { onTouchGround?.Invoke(); } //check if just landed
            if (!trackedGrounds.Contains(collision.transform)) { trackedGrounds.Add(collision.transform); } //track ground
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ValidGroundCheck(collision.transform)) {
            trackedGrounds.Remove(collision.transform); //stop tracking ground
            if (trackedGrounds.Count <= 0) { onLeaveGround?.Invoke(); } //check if just left ground
        }
    }

    private bool ValidGroundCheck(Transform toCheck)
    {
        foreach (string tag in ignoreTags) {
            if (toCheck.CompareTag(tag)) { return false; }
        }
        return true;
    }
}
