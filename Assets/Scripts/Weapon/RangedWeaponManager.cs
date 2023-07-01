using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponManager : WeaponManager
{
    private ProjectileSpawner _projectileSpawner;

    public override void Initialize(Weapon weapon)
    {
        if (transform.TryGetComponent(out _projectileSpawner))
        {
            _projectileSpawner.Initialize(weapon);
        }
        
        base.Initialize(weapon);
    }
    
    public override void Attack(Vector2 attackLocation)
    {
        _projectileSpawner.Trigger(attackLocation);
        TriggerOnAttackEffects();
        AttackRefresh();
        CheckReload();
    }
    
    public override void Attack()
    {
        _projectileSpawner.Trigger(); // shoots toward weapon forward direction
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
    
    protected override void ComputeRange()
    {
        var projectileType = _projectileSpawner.SpawnedType;

        if (projectileType == ProjectileType.Physics)
        {
            // modify in future to determine range based on physics values
            ComputedRange = Weapon.Stats.range;
            return;
        }
        
        ComputedRange = Weapon.Stats.range;
    }
}
