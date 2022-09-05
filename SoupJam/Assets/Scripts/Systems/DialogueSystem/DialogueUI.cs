using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(ResponseHandler), typeof(TypewriterEffect))]
public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueLabel, nameLabel;
    [SerializeField] GameObject dialogueBox;
    [Header("Events")]
    [SerializeField] private UnityEvent onDialogueStart;
    [SerializeField] private UnityEvent onDialogueEnd;

    //vars
    private ResponseHandler responseHandler;
    private TypewriterEffect writeEffect;

    private bool advanceQueued = false;

    //getters
    public bool IsOpen { get; private set; }
    public DialogueData CurrentData { get; private set; }

    private void Awake()
    {
        responseHandler = GetComponent<ResponseHandler>();
        writeEffect = GetComponent<TypewriterEffect>();
        //start with dialogue closed
        EndDialogue();
    }

    //------------------------state management-----------------------------
    public void ShowDialogue(DialogueData data)
    {
        IsOpen = true;
        advanceQueued = false;
        CurrentData = data;
        //activate dialogue
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(data));
        //events
        onDialogueStart?.Invoke();
    }

    public void EndDialogue()
    {
        //update vars
        IsOpen = false;
        CurrentData = null;
        //close dialogue box
        dialogueBox.SetActive(false);
        dialogueLabel.text = string.Empty;
        //events
        onDialogueEnd?.Invoke();
    }

    //---------------------events-------------------
    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    //--------------------display dialogue------------------------
    private IEnumerator StepThroughDialogue(DialogueData data)
    {
        for (int i = 0; i < data.Dialogue.Length - 1; i++) {
            HandleNameLabel(data.Dialogue[i].speaker);
            //type text on screen
            yield return RunTypingEffect(data.Dialogue[i]);
            dialogueLabel.text = data.Dialogue[i].dialogue;
            
            //wait until input
            yield return new WaitUntil(() => advanceQueued);
            advanceQueued = false; //reset input
        }
        StartCoroutine(HandleResponses(data));
    }

    private IEnumerator HandleResponses(DialogueData data)
    {
        if (data.HasResponses) {
            responseHandler.ShowResponses(data.Responses);
        }
        else {
            //wait for click to close window
            yield return new WaitUntil(() => advanceQueued);
            advanceQueued = false; //reset input
            //close window
            EndDialogue();
        }
    }

    private void HandleNameLabel(string speaker)
    {
        if (speaker == null || speaker == "") {
            nameLabel.gameObject.SetActive(false);
        }
        else {
            nameLabel.gameObject.SetActive(true);
            nameLabel.text = speaker;
        }
    }

    //----------handle typing effect---------------
    private IEnumerator RunTypingEffect(DialogueData.DialogueMessage msg)
    {
        writeEffect.Run(msg.dialogue, dialogueLabel);

        while (writeEffect.isRunning) {
            yield return null;
            //allow to skip type effect
            if (!msg.unskippable && advanceQueued) {
                writeEffect.Stop();
                advanceQueued = false; //reset input
            }
        }
    }

    //-------------------external inputs-------------------
    public void AdvanceDialogue()
    {
        if (IsOpen) {
            advanceQueued = true;
        }
    }
}
