using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Weapon
{
    private string _uid;
    private readonly WeaponData _data;
    private readonly WeaponStats _baseStats;
    private WeaponStats _activeStats;
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private WeaponManager _weaponManager;
    private bool _equipped;
    private Character _owner;
    
    public Weapon(WeaponData initialData, Character owner = null)
    {
        _uid = System.Guid.NewGuid().ToString();
        _data = initialData;
        _baseStats = _data.baseStats;
        _activeStats = _baseStats;
        _owner = owner;
    }

    public void Attack()
    {
        if(_weaponManager.Ready) _weaponManager.Attack();
    }

    public void Attack(Vector2 attackLocation)
    {
        if(_weaponManager.Ready) _weaponManager.Attack(attackLocation);
    }

    public bool ProcessHit(Collider2D collider, Vector2 hitPoint, Vector2 origin)
    {
        if (collider.transform == _owner.Transform) return false;
        
        var validHit = false;
        
        // check Transform to see if target should be hit
        validHit = Utility.LayerMaskHasLayer(_activeStats.hitDetectionMask, collider.gameObject.layer);

        if(validHit) OnHit(collider, hitPoint, origin);
        
        return validHit;
    }

    private void OnHit(Collider2D collider, Vector2 hitPosition, Vector2 origin)
    {
        // apply hit actions
        if (collider.transform.TryGetComponent<CharacterManager>(out var cm))
        {
            cm.Damage(GetDamageWithModifiers());
            cm.ReceiveHit(_owner.Transform, origin);
        }

        var collectedOnHitEffects = _activeStats.onHitEffects.Concat(_owner.Stats.onHitEffects).ToList();
        
        if(collectedOnHitEffects.Count > 0)
        {
            Debug.Log("Weapon applying on hit effects. Count: " + collectedOnHitEffects.Count);
            var charEffectArgs = new EffectArgs(_owner.Transform, collider.transform, hitPosition);
            Effect.TriggerEffectList(_owner.Stats.onHitEffects, charEffectArgs);
        }

        // play sound
        // play animation or particle effects
    }

    public void OnSpawnedProjectileDestruction(Vector2 location)
    {
        Debug.Log("PROJECTILE DESTRUCTION DELEGATE TRIGGERED!");
        if (Stats.onProjectileDestructionEffects.Count == 0) return;

        var eArgs = new EffectArgs(_owner.Transform, null, location);
        
        Effect.TriggerEffectList(Stats.onProjectileDestructionEffects, eArgs);
    }

    private int GetDamageWithModifiers()
    {
        var damage = _activeStats.damage;
        var cStats = _owner.Stats;
        damage += _data.type == WeaponType.Ranged ? cStats.rangedDamageModifier : cStats.meleeDamageModifier;
        return damage;
    }

    public virtual void Equip(Character owner)
    {
        _owner = owner;
        CreateTransform();
        
        _weaponManager = _transform.GetComponent<WeaponManager>();
        _weaponManager.Initialize(this);

        _transform.position = owner.Transform.position + _transform.localPosition;
        _transform.parent = Owner.Transform.Find("WeaponParent");
        _equipped = true;
    }

    public void Unequip()
    {
        GameObject.Destroy(_transform.gameObject);
        _transform = null;
        _weaponManager = null;
        _equipped = false;
    }

    protected Transform CreateTransform()
    {
        if (_transform != null)
        {
            return _transform;
        }

        return _transform = GameObject.Instantiate(_data.prefab).transform;
    }

    public static Weapon SpawnInWorld(WeaponData initialData, Vector2 spawnPoint)
    {
        return SpawnInWorld(new Weapon(initialData), spawnPoint);
    }

    public static Weapon SpawnInWorld(Weapon weapon, Vector2 spawnPoint)
    {
        weapon.CreateTransform();
        weapon._transform.position = spawnPoint;
        
        var wepManager = weapon._transform.GetComponent<WeaponManager>();
        wepManager.ConvertToPickup();
        wepManager.enabled = false;

        var wp = weapon._transform.AddComponent<WeaponPickup>();
        wp.Initialize(weapon.Data);
        return weapon;
    }

    public WeaponData Data => _data;
    public WeaponStats Stats => _activeStats;
    public float ComputedRange => _weaponManager.ComputedRange;
    public Transform Transform => _transform;
    public WeaponManager Manager => _weaponManager;
    public bool Equipped => _equipped;
    public Character Owner => _owner;
    public bool CanAttack => _weaponManager.Ready;
}
