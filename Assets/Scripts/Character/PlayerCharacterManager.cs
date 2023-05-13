using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacterManager : CharacterManager
{
    private List<Transform> _nearbyInteractables;

    private void Awake()
    {
        base.Awake();
        _nearbyInteractables = new List<Transform>();
    }
    
    public void Interact()
    {
        if (_nearbyInteractables.Count == 0) return;
        _nearbyInteractables[0].GetComponent<Interactable>().Interact(this);
    }
    
    private void UpdateInteractables()
    {
        if (_nearbyInteractables.Count < 2) return;
        _nearbyInteractables = _nearbyInteractables.OrderBy
        (
            i => Vector2.Distance(i.position, transform.position)
        ).ToList();
    }
    public void AddNearbyInteractable(Transform interactable)
    {
        if (_nearbyInteractables.Contains(interactable)) return;
        _nearbyInteractables.Add(interactable);
        //Debug.Log(interactable.transform.name + "added to " + name + "interactable pool");
        UpdateInteractables();
    }

    public void RemoveNearbyInteractable(Transform interactable)
    {
        if (!_nearbyInteractables.Contains(interactable)) return;
        _nearbyInteractables.Remove(interactable);
        //Debug.Log(interactable.transform.name + "removed from " + name + "interactable pool");
        UpdateInteractables();
    }
}
