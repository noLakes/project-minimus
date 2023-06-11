using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Weapon
{
    protected string uid;
    protected readonly WeaponData _data;
    protected readonly WeaponStats _baseStats;
    protected WeaponStats _activeStats;
    protected Transform _transform;
    protected SpriteRenderer _spriteRenderer;
    protected WeaponManager _weaponManager;
    protected bool _equipped;
    protected Character _owner;
    
    public Weapon(WeaponData initialData, Character owner = null)
    {
        uid = System.Guid.NewGuid().ToString();
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

        // check collider Transform to see if target should be hit
        // FIX THIS PLZ!!!
        if (collider.transform.tag == "Obstacle") validHit = true;
        else if (collider.transform.tag == "Character") validHit = true;

        if(validHit) OnHit(collider, hitPoint, origin);
        
        return validHit;
    }

    private void OnHit(Collider2D collider, Vector2 hitPosition, Vector2 origin)
    {
        // apply hit actions
        if (!collider.transform.TryGetComponent<CharacterManager>(out var cm)) return;
        cm.Damage(GetDamageWithModifiers());
        cm.ReceiveHit(_owner.Transform, origin);

        // if weapon has on hit effects
        if (_activeStats.onHitEffects.Count > 0 && cm != null)
        {
            Debug.Log("Weapon applying own on hit effects. Count: " + _activeStats.onHitEffects.Count);
            var wepEffectArgs = new EffectArgs(_transform, cm.transform, hitPosition);
            Effect.ApplyEffectList(_activeStats.onHitEffects, wepEffectArgs);
        }

        if (_owner.Stats.onHitEffects.Count > 0 && cm != null)
        {
            Debug.Log("Weapon applying owners on hit effects. Count: " + _owner.Stats.onHitEffects.Count);
            var charEffectArgs = new EffectArgs(_owner.Transform, cm.transform, hitPosition);
            Effect.ApplyEffectList(_owner.Stats.onHitEffects, charEffectArgs);
        }
        
        // play sound
        // play animation or particle effects
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
