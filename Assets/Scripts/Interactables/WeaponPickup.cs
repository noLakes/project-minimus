using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPickup : Interactable
{
    private WeaponData _weaponData;
    
    public void Initialize(WeaponData data)
    {
        _weaponData = data;
        var collider = transform.AddComponent<CircleCollider2D>();
        collider.radius = 1.5f; // can reference global parameter for weapon pickup range
        collider.isTrigger = true;
    }

    private void HandlePickup(CharacterManager cm)
    {
        cm.AddWeapon(new Weapon(_weaponData, cm.Character));
        Destroy(this.gameObject);
    }
    
    public override void Interact(CharacterManager cm)
    {
        HandlePickup(cm);
    }
}