using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Response
{
    [Tooltip("title will be displayed as text on response button")]
    [SerializeField] private string responseTitle;
    [Tooltip("follow up dialogue, if left empty, choosing response will end dialogue")]
    [SerializeField] private DialogueData dialogueData;

    //public get references
    public string Title => responseTitle;
    public DialogueData Data => dialogueData;
}
