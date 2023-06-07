using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWeaponPickup : WeaponPickup
{
    protected override void HandlePickup(CharacterManager cm)
    {
        cm.EquipSpecialWeapon(new SpecialWeapon(_weaponData));
        
        Destroy(this.gameObject);
    }
}
