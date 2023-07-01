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
        var circleCollider2D = transform.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = Game.Instance.gameGlobalParameters.itemPickupRange;
        circleCollider2D.isTrigger = true;
    }
    
    protected virtual void HandlePickup(CharacterManager cm)
    {
        cm.AddItem(_itemData);
        
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
