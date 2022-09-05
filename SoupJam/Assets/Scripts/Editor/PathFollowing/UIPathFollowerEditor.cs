using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIPathFollower))]
public class UIPathFollowerEditor : Editor
{
    UIPathFollower follower;

    private void Awake()
    {
        if (follower == null) {
            follower = target as UIPathFollower;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Refresh path points")) {
            follower.CompilePathPoints();
        }
    }
}
