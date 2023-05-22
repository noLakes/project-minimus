using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Weapon
{
    private string uid;
    private readonly WeaponData _data;
    private readonly WeaponStats _baseStats;
    private WeaponStats _activeStats;
    private float _computedRange; // for melee and physiscs weapons
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private ProjectileSpawner _projectileSpawner;
    private WeaponAttackManager _weaponAttackManager;
    private WeaponAimManager _weaponAimManager;
    private bool _equipped;
    private Character _owner;
    
    public Weapon(WeaponData initialData, Character owner = null)
    {
        uid = System.Guid.NewGuid().ToString();
        _data = initialData;
        _baseStats = new WeaponStats(initialData);
        _activeStats = _baseStats;
        _owner = owner;
    }

    public void Attack()
    {
        if (!_weaponAttackManager.Ready) return;
        
        switch (_data.type)
        {
            case WeaponType.Melee:
            {
                _animator.SetTrigger("Attack");
                _weaponAttackManager.OnWeaponAttack();
                _weaponAimManager.PauseAiming();
                break;
            }
            case WeaponType.Ranged:
            {
                _projectileSpawner.Spawn(); // shoots toward weapon forward direction
                _weaponAttackManager.OnWeaponAttack();
                break;
            }
        }
    }

    public void Attack(Vector2 attackLocation)
    {
        if (!_weaponAttackManager.Ready) return;
        
        switch (_data.type)
        {
            case WeaponType.Melee:
            {
                _animator.SetTrigger("Attack");
                _weaponAttackManager.OnWeaponAttack();
                _weaponAimManager.PauseAiming();
                break;
            }
            case WeaponType.Ranged:
            {
                _projectileSpawner.Spawn(attackLocation); // shoots towards location regardless of aim
                _weaponAttackManager.OnWeaponAttack();
                break;
            }
        }
    }

    public bool ProcessHit(Collider2D collider, Vector2 hitPoint)
    {
        var validHit = false;

        // check collider Transform to see if target should be hit
        if (collider.transform.tag == "Obstacle") validHit = true;
        else if (collider.transform.tag == "Character") validHit = true;

        if(validHit) OnHit(collider, hitPoint);
        
        return validHit;
    }

    private void OnHit(Collider2D collider, Vector2 hitPosition)
    {
        // apply hit actions
        if (!collider.transform.TryGetComponent<CharacterManager>(out var cm)) return;
        cm.Damage(_activeStats.Damage);
            
        // if weapon has on hit effects
        if (_activeStats.OnHitEffects.Count > 0)
        {
            // generate effect args s truct
            var effectArgs = new EffectArgs(_transform, cm.transform, hitPosition);

            foreach(var e in _activeStats.OnHitEffects)
            {
                e.Apply(effectArgs);
            }
        }
        
        // play sound
        // play animation or particle effects
    }

    public void OnAttackAnimationStart()
    {
        // do something
    }
    
    public void OnAttackAnimationEnd()
    {
        _weaponAimManager.ResumeAiming();
    }

    public void Equip(Character owner)
    {
        _owner = owner;
        CreateTransform();
        
        if (_transform.TryGetComponent(out _projectileSpawner))
        {
            _projectileSpawner.Initialize(this);
        }

        _weaponAttackManager = _transform.GetComponent<WeaponAttackManager>();
        _weaponAttackManager.Initialize(this);

        _weaponAimManager = Owner.Transform.GetComponentInChildren<WeaponAimManager>();
        _weaponAimManager.ResetPosition();
        _weaponAimManager.ResumeAiming(); // prevents weapon aim staying stuck mid melee swing from previous equipped weapon
        _spriteRenderer = _transform.GetComponent<SpriteRenderer>();
        
        if (_transform.TryGetComponent(out _animator))
        {
            _transform.GetComponent<WeaponAnimationHelper>()?.Initialize(this);
        }
        
        _transform.position = owner.Transform.position + _transform.localPosition;
        var parent = Owner.Transform.Find("WeaponParent");
        _transform.parent = parent;
        ComputeRange();
        _equipped = true;
    }

    public void Unequip()
    {
        GameObject.Destroy(_transform.gameObject);
        _transform = null;
        _projectileSpawner = null;
        _weaponAttackManager = null;
        _weaponAimManager = null;
        _equipped = false;
    }

    private Transform CreateTransform()
    {
        if (_transform != null)
        {
            return _transform;
        }

        return _transform = GameObject.Instantiate(_data.prefab).transform;
    }

    private void ToggleAttackComponentsActive(bool status)
    {
        if (_transform.TryGetComponent<Animator>(out var anim)) anim.enabled = status;
        if (_transform.TryGetComponent<WeaponAttackManager>(out var attackMan)) attackMan.enabled = status;
        if (_transform.TryGetComponent<WeaponAnimationHelper>(out var animHelp)) animHelp.enabled = status;
    }

    public static Weapon SpawnInWorld(WeaponData initialData, Vector2 spawnPoint)
    {
        var weapon = new Weapon(initialData);
        weapon.CreateTransform();
        weapon._transform.position = spawnPoint;
        weapon.ToggleAttackComponentsActive(false);
        
        var wp = weapon._transform.AddComponent<WeaponPickup>();
        wp.Initialize(weapon.Data);
        return weapon;
    }

    public static Weapon SpawnInWorld(Weapon weapon, Vector2 spawnPoint)
    {
        weapon.CreateTransform();
        weapon._transform.position = spawnPoint;
        weapon.ToggleAttackComponentsActive(false);
        
        var wp = weapon._transform.AddComponent<WeaponPickup>();
        wp.Initialize(weapon.Data);
        return weapon;
    }

    public int GetFactionLayerMask()
    {
        return _owner == Game.Instance.PlayerCharacter.Character
            ? Game.Instance.TargetEnemyHitScanMask
            : Game.Instance.TargetPlayerHitScanMask;
    }

    private void ComputeRange()
    {
        switch (_data.type)
        {
            case WeaponType.Melee:
            {
                float ownerHalfSize = _owner.Transform.GetComponent<CharacterManager>().Size / 2;
                Debug.Log("ComputeRange: " + ownerHalfSize + " / " + _weaponAttackManager.HitRadius * 1.9f);
                _computedRange = (_weaponAttackManager.HitRadius * 1.95f) + ownerHalfSize;
                break;
            }
            case WeaponType.Ranged:
            {
                // not implemented yet
                _computedRange = _activeStats.Range; // replace this
                break;
            }
        }
        Debug.Log("ComputedRange: " + _computedRange);
    }
    
    public WeaponData Data => _data;
    public WeaponStats Stats => _activeStats;
    public float ComputedRange => _computedRange;
    public Transform Transform => _transform;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public bool Equipped => _equipped;
    public Character Owner => _owner;
    public bool CanAttack => _weaponAttackManager.Ready;
}
