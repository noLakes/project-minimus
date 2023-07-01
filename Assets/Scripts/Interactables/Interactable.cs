using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class Interactable : MonoBehaviour
{
    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public abstract void Interact(CharacterManager cm);
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerCharacterManager>(out var pCM))
        {
            pCM.AddNearbyInteractable(transform);
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerCharacterManager>(out var pCM))
        {
            pCM.RemoveNearbyInteractable(transform);
        }
    }
}
