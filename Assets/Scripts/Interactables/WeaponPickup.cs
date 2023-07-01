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
        var circleCollider2D = transform.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = Game.Instance.gameGlobalParameters.itemPickupRange;
        circleCollider2D.isTrigger = true;
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
