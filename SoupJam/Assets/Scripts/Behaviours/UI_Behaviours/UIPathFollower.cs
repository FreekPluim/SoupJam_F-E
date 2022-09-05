using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPathFollower : MonoBehaviour
{
    [System.Serializable]
    private struct PathPoint {
        public RectTransform transform;
        [Tooltip("Time to wait before moving on to the next point in the path.")]
        public float delay;
    }

    [System.Serializable]
    private enum LoopMode {
        Reset, Loop, Bounce
    }

    [System.Serializable]
    private enum StepMode {
        Step, Cycle, Continuous
    }

    //settings
    [Header("movement settings")]
    [SerializeField] private float moveSpeed;

    [Tooltip("Dictates how many steps are taken along the path when StartMove() is called.\n\n" +
        "Step: the object takes a single step along the path.\n" +
        "Cycle: the object follows the entire path once.\n" +
        "Continuous: the object follows the path untill told to stop.")]
    [SerializeField] private StepMode stepMode;

    [Tooltip("Dictates what happens when the object reaches the end of the path.\n\n" +
        "Reset: When the object reaches the end of the path, it is teleported back to the first point.\n" +
        "Loop: When the object reaches the end of the path, it travels back to the first point.\n" +
        "Bounce: When the object reaches the end of the path, it gets sent back through the path in reverse order")]
    [SerializeField] private LoopMode loopMode;

    [Header("path settings")]
    [SerializeField] private bool moveOnStart;

    [Space(10f)]
    [SerializeField] private List<PathPoint> path;
    [Tooltip("==OPTIONAL==\n\n" +
        "Rect transform that holds all spawnpoint transforms. used to auto compile spawnpoint list.")]
    [SerializeField] private RectTransform pathHolder;

    //vars
    private RectTransform rt;
    private int currentPathIndex;
    //states
    private bool moving;
    private bool waiting;
    private bool movingForward;

    private void Start()
    {
        //var checks
        if (path == null || path.Count <= 0) { Debug.LogError("Please assign a UI path"); }
        rt = GetComponent<RectTransform>();
        if (rt == null) { Debug.LogError("No target Rect Transform found"); }
        //start move
        movingForward = true;
        if (moveOnStart) { StartMove(); }
    }

    private void StartMove()
    {
        moving = true;
    }

    public void EndMove()
    {
        moving = false;
    }

    //----------movement-----------
    private void Update()
    {
        if (moving && !waiting) {
            Move();
        }
    }

    private void Move()
    {
        Vector2 targetPos = path[currentPathIndex].transform.anchoredPosition;
        //move towards target pos
        rt.anchoredPosition = Vector2.MoveTowards(rt.anchoredPosition, targetPos, moveSpeed * 100 * Time.deltaTime);
        if (Vector2.Distance(rt.anchoredPosition, targetPos) < 0.1f) {
            rt.anchoredPosition = targetPos; //set pos
            OnReachPoint();
        }
    }

    //--------------Reach Point-------------
    private void OnReachPoint()
    {
        StartCoroutine(OnReachPointCo());
        UpdatePathIndex();
        //reach end check
        if (ReachedEnd()) {
            OnReachEnd();
        }
        //stop move check
        if (stepMode == StepMode.Step) { moving = false; } //if step mode, stop moving
    }
    private void UpdatePathIndex()
    {
        currentPathIndex += movingForward ? 1 : -1;
    }

    private bool ReachedEnd()
    {
        if (currentPathIndex < 0 || currentPathIndex >= path.Count) {
            return true;
        }
        return false;
    }

    private IEnumerator OnReachPointCo()
    {
        waiting = true;
        yield return new WaitForSeconds(path[currentPathIndex].delay);
        waiting = false;
    }

    //----------------Reach end of path-------------
    private void OnReachEnd()
    {
        if (stepMode == StepMode.Cycle) { moving = false; }
        switch (loopMode) {
            case LoopMode.Reset:
                currentPathIndex = 0;
                rt.anchoredPosition = path[currentPathIndex].transform.anchoredPosition;
                break;

            case LoopMode.Loop:
                currentPathIndex = 0;
                break;

            case LoopMode.Bounce:
                movingForward = !movingForward;
                currentPathIndex = movingForward ? 1 : path.Count - 2;
                break;
        }
    }

    //---------auto compile---------
    public void CompilePathPoints()
    {
        if (pathHolder != null) {
            path.Clear();
            foreach (RectTransform child in pathHolder) {
                path.Add(new PathPoint { transform = child });
            }
        }
    }
}
