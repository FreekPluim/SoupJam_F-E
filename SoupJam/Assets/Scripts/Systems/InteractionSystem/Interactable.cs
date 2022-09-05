using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;

    private List<Interactor> interactors;

    private void Start()
    {
        interactors = new List<Interactor>();
    }

    //--------------remove management-----------
    public void DestroyInteractable()
    {
        foreach (Interactor interactor in interactors) {
            interactor.RemoveInteractable(this);
        }
        Destroy(gameObject);
    }

    //-------------trigger events--------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Interactor interactor)) {
            interactors.Add(interactor);
            interactor.AddInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactor interactor)) {
            interactors.Remove(interactor);
            interactor.RemoveInteractable(this);
        }
    }
}
