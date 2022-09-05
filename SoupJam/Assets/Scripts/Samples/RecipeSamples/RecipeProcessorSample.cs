using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeProcessorSample : RecipeProcessor<string, string>
{
    [SerializeField] private List<string> inputs;

    private void Start()
    {
        Debug.Log(TryRecipe(inputs));
    }
}
