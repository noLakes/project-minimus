using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponManager : WeaponManager
{
    private ProjectileSpawner _projectileSpawner;

    public override void Initialize(Weapon weapon)
    {
        base.Initialize(weapon);
        if (transform.TryGetComponent(out _projectileSpawner))
        {
            _projectileSpawner.Initialize(Weapon);
        }
    }
    
    public override void Attack(Vector2 attackLocation)
    {
        _projectileSpawner.Spawn(attackLocation);
        TriggerOnAttackEffects();
        AttackRefresh();
        CheckReload();
    }
    
    public override void Attack()
    {
        _projectileSpawner.Spawn(); // shoots toward weapon forward direction
        TriggerOnAttackEffects();
        AttackRefresh();
        CheckReload();
    }
    
    public override void ConvertToPickup()
    {
        base.ConvertToPickup();
        Destroy(_projectileSpawner);
        _projectileSpawner = null;
    }
}
