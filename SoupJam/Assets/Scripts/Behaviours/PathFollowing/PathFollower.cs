using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [System.Serializable]
    public struct PathPoint {
        public Transform point;
        [Tooltip("Time to wait before moving on to the next point in the path.")]
        public float delay;
    }

    public enum RotateMode {
        None, Look_ahead, Use_point_rotation
    }
    public enum StepMode { 
        Step, Cycle, Continuous
    }
    public enum LoopMode {
        Reset, Loop, Bounce
    }

    //settings
    [Header("movement settings")]
    [SerializeField] private float moveSpeed;

    [Tooltip("Dictates how many steps are taken along the path when StartMove() is called.\n\n" +
        "Step: the object takes a single step along the path.\n" +
        "Cycle: the object follows the entire path once.\n" +
        "Continuous: the object follows the path untill told to stop.")]
    public StepMode stepMode;

    [Tooltip("Dictates what happens when the object reaches the end of the path.\n\n" +
        "Reset: When the object reaches the end of the path, it is teleported back to the first point.\n" +
        "Loop: When the object reaches the end of the path, it travels back to the first point.\n" +
        "Bounce: When the object reaches the end of the path, it gets sent back through the path in reverse order")]
    public LoopMode loopMode;

    [Header("rotation settings")]
    [SerializeField] private float rotateSpeed;

    [Tooltip("Dictates how rotation is handled while following the path\n\n" +
        "None: the object will not rotate\n" +
        "Look_ahead: the object will look towards the next point in the path\n" +
        "Use_point_rotateion: the object will look towards the rotation of the next point in the path")]
    public RotateMode rotateMode;

    [Header("path settings")]
    [SerializeField] private bool moveOnStart;

    [Space(10f)]
    public List<PathPoint> path;
    [Tooltip("==OPTIONAL==\n\n" +
        "Transform that holds all spawnpoint transforms. used to auto compile spawnpoint list.")]
    [SerializeField] private Transform pathHolder;

    //vars
    private int currentPathIndex;
    //states
    private bool moving;
    private bool waiting;
    private bool movingForward;
    //rotate vars
    private Quaternion targetRotation;

    //----------------start moving-------------
    private void Start()
    {
        //path check
        if (path == null || path.Count == 0) { Debug.LogError("No path was set on" + transform.name +"!"); return; }
        //base vars
        movingForward = true;
        if (moveOnStart) { StartMove(); }
    }

    public void StartMove()
    {
        moving = true;
        SetRotateTarget();
    }

    public void StopMove()
    {
        moving = false;
    }

    //--------------Movement---------------
    private void Update()
    {
        if (moving && !waiting) {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, path[currentPathIndex].point.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, path[currentPathIndex].point.position) < 0.1f) {
            transform.position = path[currentPathIndex].point.position; //set pos
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
        SetRotateTarget();
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
                transform.position = path[currentPathIndex].point.position;
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

    //-------------Rotation-------------
    private void SetRotateTarget()
    {
        switch (rotateMode) {
            case RotateMode.Look_ahead:
                targetRotation = Quaternion.LookRotation(path[currentPathIndex].point.position - transform.position);
                break;

            case RotateMode.Use_point_rotation:
                targetRotation = path[currentPathIndex].point.rotation;
                break;
        }
    }

    private void Rotate()
    {
        if (rotateMode != RotateMode.None) {
            //rotate towards target
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    //---------auto compile---------
    public void CompilePathPoints()
    {
        if (pathHolder != null) {
            path.Clear();
            foreach (Transform child in pathHolder) {
                path.Add(new PathPoint { point = child });
            }
        }
    }
}
