using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedChanceSample : MonoBehaviour
{
    [SerializeField] WeightedChance<string> logs;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            Debug.Log(logs.GetRandomEntry());
        }
    }
}
