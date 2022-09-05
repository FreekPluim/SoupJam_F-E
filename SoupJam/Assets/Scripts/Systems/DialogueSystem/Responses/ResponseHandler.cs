using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
    [Tooltip("RectTransform that holds the responseContainer. \nThis element resizes to account for responses")]
    [SerializeField] private RectTransform responseBox;
    [Tooltip("RectTransform that should be held be the responseBox. \nThis element holds the response buttons")]
    [SerializeField] private RectTransform responseContainer;
    [SerializeField] private RectTransform buttonTemplate;

    private DialogueUI dialogueUI;
    private ResponseEvent[] responseEvents;

    private readonly List<GameObject> tempButtons = new();

    private void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
    }

    //-----------------------handle events----------------------------
    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents;
    }

    //-----------------------Handle Responses------------------------------
    public void ShowResponses(Response[] responses)
    {
        for (int i = 0; i < responses.Length; i++) {
            BuildResponseButton(responses[i], i);
        }
        responseBox.gameObject.SetActive(true);
        //configure response box
        float boxHeight = buttonTemplate.sizeDelta.y * responses.Length;
        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, boxHeight);
    }

    private void BuildResponseButton(Response response, int index)
    {
        GameObject button = Instantiate(buttonTemplate, responseContainer).gameObject;
        button.SetActive(true);
        button.GetComponentInChildren<TMP_Text>().text = response.Title; //not particularly optimized, but allows for some freedom
        button.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, index));
        //register button
        tempButtons.Add(button);
    }

    //----------------------Handle Response Events------------------
    private void OnPickedResponse(Response response, int responseIndex)
    {
        //reset buttons
        responseBox.gameObject.SetActive(false);
        ResetButtons();
        HandleResponseEvent(responseIndex);
        HandleFollowUpDialogue(response);
    }

    private void ResetButtons()
    {
        foreach (GameObject button in tempButtons) {
            Destroy(button);
        }
        tempButtons.Clear();
    }

    private void HandleResponseEvent(int responseIndex)
    {
        //check if event is in bounds
        if (responseEvents != null && responseIndex <= responseEvents.Length) {
            responseEvents[responseIndex].OnResponse?.Invoke();
        }
        responseEvents = null; //prevent carrying of events between dialogues on same object
    }

    private void HandleFollowUpDialogue(Response response)
    {
        if (response.Data) {
            dialogueUI.ShowDialogue(response.Data);
        }
        else {
            dialogueUI.EndDialogue();
        }
    }
}