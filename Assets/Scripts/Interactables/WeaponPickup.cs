using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPickup : Interactable
{
    protected WeaponData _weaponData;
    
    public void Initialize(WeaponData data)
    {
        _weaponData = data;
        var collider = transform.AddComponent<CircleCollider2D>();
        collider.radius = 1.5f; // can reference global parameter for weapon pickup range
        collider.isTrigger = true;
    }

    protected virtual void HandlePickup(CharacterManager cm)
    {
        if (cm.Character.Weapons.Count == cm.Character.Stats.maxWeaponCount)
        {
            // handle dropping current weapon
            cm.DropWeapon();
            var newWeapon = cm.AddWeapon(new Weapon(_weaponData, cm.Character));
            cm.EquipWeapon(newWeapon);
        }
        else
        {
            cm.AddWeapon(new Weapon(_weaponData, cm.Character));
        }
        
        Destroy(this.gameObject);
    }
    
    public override void Interact(CharacterManager cm)
    {
        HandlePickup(cm);
    }
}
