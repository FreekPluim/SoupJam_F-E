using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleMover : MonoBehaviour
{
    private enum MoveMode {
        Linear, Lerp
    }

    [Header("Settings")]
    [SerializeField] private RectTransform endPosition;
    [SerializeField] private MoveMode moveMode;
    [Space(10f)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool moveOnStart;

    //vars
    private RectTransform rt;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 targetPos;
    //states
    private bool moving;
    private bool movingToEnd;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        if (rt == null) { Debug.LogError("No UI element detected, make sure " + transform.name + " is a UI element!"); }
        //set positions
        startPos = rt.anchoredPosition;
        if (endPosition == null) { Debug.LogError("No end position set on " + transform.name + "!"); }
        endPos = endPosition.anchoredPosition;
        //move if moveOnStart
        if (moveOnStart) StartMove();
    }

    //-------------movement------------
    public void StartMove()
    {
        moving = true;
        //initialize target vars
        movingToEnd = !movingToEnd;
        targetPos = movingToEnd ? endPos : startPos;
    }

    private void Update()
    {
        if (moving) {
            Move();
        }
    }

    private void Move()
    {
        switch (moveMode) {
            case MoveMode.Linear: MoveLinear(); break;
            case MoveMode.Lerp: MoveLerp(); break;
        }
        ReachEndCheck();
    }

    private void MoveLinear()
    {
        rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, targetPos, moveSpeed * 100 * Time.deltaTime);
    }

    private void MoveLerp()
    {
        rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, targetPos, moveSpeed * Time.deltaTime);
    }

    //--------------reach end------------
    private void ReachEndCheck()
    {
        if (Vector2.Distance(rt.anchoredPosition, targetPos) < 0.1f) {
            OnReachEnd();
        }
    }

    private void OnReachEnd()
    {
        moving = false;
    }
}
