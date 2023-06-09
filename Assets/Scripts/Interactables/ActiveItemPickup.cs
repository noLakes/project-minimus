using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItemPickup : WeaponPickup
{
    protected override void HandlePickup(CharacterManager cm)
    {
        cm.EquipSpecialWeapon(new ActiveItem(_weaponData));
        
        Destroy(this.gameObject);
    }
}
