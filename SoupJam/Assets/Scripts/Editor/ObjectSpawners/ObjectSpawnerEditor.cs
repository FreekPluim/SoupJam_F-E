using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor
{
    ObjectSpawner spawner;

    private void Awake()
    {
        if (spawner == null) {
            spawner = target as ObjectSpawner;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Refresh spawn points")) {
            spawner.CompileSpawnPoints();
        }
    }
}
