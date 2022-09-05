using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathFollower))]
public class PathFollowerEditor : Editor
{
    PathFollower follower;

    private void Awake()
    {
        if (follower == null) {
            follower = target as PathFollower;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Refresh path points")) {
            follower.CompilePathPoints();
        }
    }

    private void OnSceneGUI()
    {
        //draw path
        Handles.color = Color.green;
        for (int i = 0; i < follower.path.Count - 1; i++) {
            Handles.DrawLine(follower.path[i].point.position, follower.path[i + 1].point.position);
        }
        //draw loop line
        if (follower.loopMode == PathFollower.LoopMode.Loop) {
            Handles.DrawLine(follower.path[0].point.position, follower.path[^1].point.position);
        }
        //draw point rotation
        Handles.color = Color.yellow;
        if (follower.rotateMode == PathFollower.RotateMode.Use_point_rotation) {
            for (int i = 0; i < follower.path.Count; i++) {
                Transform t = follower.path[i].point;
                Handles.DrawLine(t.position, t.position + t.forward, 2f);
            }
        }
    }
}
