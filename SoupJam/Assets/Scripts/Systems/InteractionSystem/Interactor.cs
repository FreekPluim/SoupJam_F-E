using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    [Header("Events")]
    [Tooltip("triggers when the interactor comes in range of the first interactable")]
    [SerializeField] private UnityEvent onCanInteract;
    [Tooltip("triggers when the interactor leaves the range of the last interactable")]
    [SerializeField] private UnityEvent onStopCanInteract;
    //vars
    private List<Interactable> interactables;

    private void Start()
    {
        interactables = new List<Interactable>();
    }

    //---------------interactables management------------------
    public void AddInteractable(Interactable interactable)
    {
        interactables.Add(interactable);
        if (interactables.Count > 1) {
            SortInteractables();
        }
        else { onCanInteract?.Invoke(); }
    }

    public void RemoveInteractable(Interactable interactable)
    {
        interactables.Remove(interactable);
        if (interactables.Count > 1) {
            SortInteractables();
        }
        else if (interactables.Count == 0) { onStopCanInteract.Invoke(); }
    }

    private void SortInteractables()
    {
        interactables.Sort((Interactable a, Interactable b) => 
            Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position))
        );
    }

    //----------------interact with interactable---------------
    public void TryInteract()
    {
        Debug.Log("interactables: " + interactables.Count);
        if (interactables.Count > 0) {
            interactables[0].onInteract?.Invoke();
        }
    }
}
