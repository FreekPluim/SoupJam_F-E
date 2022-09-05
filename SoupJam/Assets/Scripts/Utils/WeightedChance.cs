using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedChance<T>
{
    [System.Serializable]
    private struct WeightedOption {
        public T option;
        public float chance;
    }

    [SerializeField] private List<WeightedOption> chances;

    //vars
    private float totalChance;

    //-------------compile options----------------
    private void CalcTotalChance()
    {
        totalChance = 0;
        foreach (WeightedOption option in chances) {
            totalChance += option.chance;
        }
    }

    //--------------random chance----------------
    public T GetRandomEntry()
    {
        if (totalChance <= 0f) { CalcTotalChance(); }
        //choose random option
        T chosenOption = default;
        float rand = Random.Range(0, totalChance);
        for (int i = 0; i < chances.Count; i++) {
            //found chosen option
            if (rand < chances[i].chance) {
                chosenOption = chances[i].option;
                break;
            }
            rand -= chances[i].chance;
        }
        return chosenOption;
    }
}
