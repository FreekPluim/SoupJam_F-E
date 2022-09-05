using UnityEngine;
using System;

public class DialogueResponseEvents : MonoBehaviour
{
    [System.Serializable]
    public class ResponseLink {
        [HideInInspector] public string name;
        public DialogueData data;
        public ResponseEvent[] responseEvents;
    }

    public ResponseLink[] eventLinks;

    public void OnValidate()
    {
        if (eventLinks == null) return; 
        foreach (ResponseLink response in eventLinks) {
            //are there responses to attach events to?
            if (response.data == null) return;
            if (response.data.Responses == null) return;
            //are events already generated?
            if (eventLinks != null && eventLinks.Length == response.data.Responses.Length) return;

            InitializeEventsList(response);
            GenerateEvents(response);
        }
    }

    private void InitializeEventsList(ResponseLink r)
    {
        if (r.responseEvents == null) {
            r.responseEvents = new ResponseEvent[r.data.Responses.Length];
        }
        else {
            Array.Resize(ref r.responseEvents, r.data.Responses.Length);
        }
    }

    private void GenerateEvents(ResponseLink r)
    {
        r.name = r.data.name;
        //create events
        for (int i = 0; i < r.data.Responses.Length; i++) {
            Response response = r.data.Responses[i];
            if (r.responseEvents[i] != null) {
                //update event
                r.responseEvents[i].name = response.Title;
            }
            else {
                //create new event
                r.responseEvents[i] = new ResponseEvent() { name = response.Title };
            }
        }
    }
}