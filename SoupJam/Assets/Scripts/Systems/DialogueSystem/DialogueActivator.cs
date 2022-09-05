using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueResponseEvents))]
public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private DialogueData data;

    //vars
    private DialogueResponseEvents responseEvents;


    private void Awake()
    {
        //get external components
        responseEvents = GetComponent<DialogueResponseEvents>();
    }

    //----------------start dialgue--------------
    public void StartDialogue(DialogueUI targetUI)
    {
        //subscribe response events
        foreach (DialogueResponseEvents.ResponseLink link in responseEvents.eventLinks) {
            if (link.data == data) {
                targetUI.AddResponseEvents(link.responseEvents);
                break;
            }
        }
        //start dialogue
        targetUI.ShowDialogue(data);
    }

    //-------------manage data-------------
    public void SetDialogueData(DialogueData data)
    {
        this.data = data;
    }
}
