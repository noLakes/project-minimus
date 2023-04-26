using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon
{
    private string uid;
    private WeaponData _data;
    public WeaponData Data { get => _data; }
    private WeaponStats _baseStats;
    private WeaponStats _activeStats;
    public WeaponStats Stats { get => _activeStats; }
    public Transform Transform { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    private Animator _animator;
    private ProjectileSpawner _projectileSpawner;
    private WeaponAttackManager _weaponAttackManager;
    private WeaponAimManager _weaponAimManager;

    private bool _equipped;
    public bool Equipped { get => _equipped; }
    public Character Owner { get; protected set; }

    public Weapon(WeaponData initialData, Character owner)
    {
        uid = System.Guid.NewGuid().ToString();
        _data = initialData;
        _baseStats = new WeaponStats(initialData);
        _activeStats = _baseStats;
        this.Owner = owner;
    }

    public void Attack(Vector2 attackLocation)
    {
        if(_data.type == WeaponType.Melee) // melee attack
        {
            if (_weaponAttackManager.Ready)
            {
                _animator.SetTrigger("Attack");
                _weaponAttackManager.OnWeaponAttack();
                _weaponAimManager.PauseAiming();
            }
        }
        else if (_data.type == WeaponType.Ranged) // ranged attack
        {
            if (_weaponAttackManager.Ready)
            {
                // trigger attack
                _projectileSpawner.Spawn(attackLocation, Quaternion.identity);

                // notify WeaponAttackManager
                _weaponAttackManager.OnWeaponAttack();
            }
        }
    }

    public bool ValidateHit(Collider2D collider, Vector2 hitPosition)
    {
        bool validHit = false;

        // check collider Transform to see if target should be hit
        if (collider.transform.tag == "Obstacle") validHit = true;
        else if (collider.transform.tag == "Character") validHit = true;

        if(validHit) OnHit(collider, hitPosition);
        
        return validHit;
    }

    private void OnHit(Collider2D collider, Vector2 hitPosition)
    {
        // apply hit actions
        if(collider.transform.TryGetComponent<CharacterManager>(out CharacterManager c))
        {
            Debug.Log("Damaging " + c.transform.name);
            c.Damage(_activeStats.damage);
            
            // if weapon has on hit effects
            if (_activeStats.onHitEffects.Count > 0)
            {
                // generate effect args s truct
                EffectArgs args = new EffectArgs(Transform, c.transform, hitPosition);

                foreach(Effect e in _activeStats.onHitEffects)
                {
                    e.Apply(args);
                }
            }
        }

        // to do
        // play sound
        // play animation or effect
    }

    public void OnAttackAnimationStart()
    {
        // do something
    }
    public void OnAttackAnimationEnd()
    {
        _weaponAimManager.ResumeAiming();
    }

    public void Equip()
    {
        Debug.Log("Weapon equipped");
        GameObject g = GameObject.Instantiate(_data.prefab) as GameObject;
        Transform = g.transform;
        
        _projectileSpawner = Transform.GetComponent<ProjectileSpawner>();
        if(_projectileSpawner != null) _projectileSpawner.Initialize(this);

        _weaponAttackManager = Transform.GetComponent<WeaponAttackManager>();
        _weaponAttackManager.Initialize(this);

        _weaponAimManager = Owner.Transform.GetComponentInChildren<WeaponAimManager>();

        spriteRenderer = Transform.GetComponent<SpriteRenderer>();
        _animator = Transform.GetComponent<Animator>();
        
        if(_animator != null) Transform.GetComponent<WeaponAnimationHelper>()?.Initialize(this);

        Transform.parent = Owner.Transform.Find("WeaponParent");

        _equipped = true;
    }

    public void Unequip()
    {
        GameObject.Destroy(Transform.gameObject);
        Transform = null;
        _projectileSpawner = null;
        _weaponAttackManager = null;
        _weaponAimManager = null;
        
        _equipped = false;
    }
}
