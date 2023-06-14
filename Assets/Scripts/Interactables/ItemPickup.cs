using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : Interactable
{
    private ItemData _itemData;

    public void Initialize(ItemData data)
    {
        _itemData = data;
        var collider = transform.AddComponent<CircleCollider2D>();
        collider.radius = 1.5f; // can reference global parameter for item pickup range
        collider.isTrigger = true;
    }
    
    protected virtual void HandlePickup(CharacterManager cm)
    {
        //add to character based on item type
        // TO DO
        
        Destroy(this.gameObject);
    }

    public override void Interact(CharacterManager cm)
    {
        HandlePickup(cm);
    }

    public static void Create(ItemData data, Vector2 location)
    {
        Transform tr = GameObject.Instantiate(data.pickupPrefab, location, Quaternion.identity).transform;
        var collider = tr.AddComponent<CircleCollider2D>();
        collider.radius = 1.5f; // update to reference global pickup range in future
        collider.isTrigger = true;

        var pickup = tr.AddComponent<ItemPickup>();
        pickup.Initialize(data);
    }
}
