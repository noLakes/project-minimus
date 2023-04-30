using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private UnityEvent _interactionEvent;

    public void Interact()
    {
        _interactionEvent.Invoke();
    }

    public void AddInteractionListener(UnityAction action)
    {
        _interactionEvent.AddListener(action);
    }

    public void RemoveInteractionListener(UnityAction action)
    {
        _interactionEvent.RemoveListener(action);
    }
}
