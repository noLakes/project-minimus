using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveItemPickup : Interactable
{
    private PassiveItemData _passiveItemData;
    
    public void Initialize(PassiveItemData data)
    {
        _passiveItemData = data;
        var collider = transform.AddComponent<CircleCollider2D>();
        collider.radius = 1.5f; // can reference global parameter for weapon pickup range
        collider.isTrigger = true;
    }
    
    private void HandlePickup(CharacterManager cm)
    {
        if (cm.Character.HasPassiveItem(_passiveItemData.code)) return;
        cm.Character.AddPassiveItem(new PassiveItem(_passiveItemData));
        Destroy(this.gameObject);
    }
    
    public override void Interact(CharacterManager cm)
    {
        HandlePickup(cm);
    }
}
