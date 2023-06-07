using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialWeapon : Weapon
{
    public SpecialWeapon(WeaponData initialData, Character owner = null) : base(initialData, owner)
    {
        if(_spriteRenderer != null) _spriteRenderer.enabled = false;
    }

    public override void Equip(Character owner)
    {
        // override in case and changes need to be made
        _owner = owner;
        CreateTransform();

        _weaponManager = _transform.GetComponent<WeaponManager>();
        _weaponManager.Initialize(this);
        
        _transform.position = owner.Transform.position;
        _transform.parent = Owner.Transform.Find("WeaponParent");
        _equipped = true;
    }

    public static SpecialWeapon SpawnInWorld(WeaponData initialData, Vector2 spawnPoint)
    {
        return SpawnInWorld(new SpecialWeapon(initialData), spawnPoint);
    }

    public static SpecialWeapon SpawnInWorld(SpecialWeapon sWeapon, Vector2 spawnPoint)
    {
        sWeapon.CreateTransform();
        sWeapon._transform.position = spawnPoint;
        
        var wepManager = sWeapon._transform.GetComponent<WeaponManager>();
        wepManager.ConvertToPickup();
        wepManager.enabled = false;
        
        var swp = sWeapon._transform.AddComponent<SpecialWeaponPickup>();
        swp.Initialize(sWeapon.Data);
        return sWeapon;
    }
}
