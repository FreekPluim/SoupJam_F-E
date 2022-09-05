using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaveSpawner))]
public class WaveSpawnerEditor : Editor
{
    WaveSpawner spawner;

    private void Awake()
    {
        if (spawner == null) {
            spawner = target as WaveSpawner;
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
