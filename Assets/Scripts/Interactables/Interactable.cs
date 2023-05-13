using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact(CharacterManager cm);
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<PlayerCharacterManager>().AddNearbyInteractable(transform);
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<PlayerCharacterManager>().RemoveNearbyInteractable(transform);
    }
    
    /*
    // could be useful for AI in future
    public static List<Interactable> CollectNearbyInteractables(Vector2 point, float radius)
    {
        Collider2D[] c = Physics2D.OverlapCircleAll(point, radius, Game.INTERACTABLE_MASK);
        List<Interactable> interactablesInRange = new List<Interactable>();

        if (c.Length < 1) return interactablesInRange;

        foreach (var t in c)
        {
            interactablesInRange.Add(t.GetComponent<Interactable>());
        }

        return interactablesInRange;
    }
    */
}
