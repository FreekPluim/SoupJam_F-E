using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public struct DialogueMessage {
        public string speaker;
        [TextArea] public string dialogue;
        public bool unskippable;
    }

    //vars
    [SerializeField] private DialogueMessage[] dialogue;
    [SerializeField] private Response[] responses;

    //--------------------------getter functions---------------------------------
    public DialogueMessage[] Dialogue => dialogue;
    public Response[] Responses => responses;
    //responseCheck
    public bool HasResponses => responses != null && responses.Length > 0;
}
