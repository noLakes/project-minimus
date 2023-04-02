using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    protected string uid;
    WeaponData _data;
    public WeaponData Data { get => _data; }

    WeaponStats _baseStats;
    WeaponStats _activeStats;
    public WeaponStats Stats { get => _activeStats; }
    
    public Transform Transform { get; private set; }
    ProjectileSpawner _projectileSpawner;
    ReloadManager _reloadManager;

    bool _equipped;
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
            
        }
        else if(_data.type == WeaponType.Ranged)// ranged attack
        {
            if(_reloadManager.Ready)
            {
                // trigger attack
                _projectileSpawner.Spawn(attackLocation, Quaternion.identity);

                // notify ReloadManager
                _reloadManager.OnWeaponAttack();
            }
        }
    }

    public bool ValidateHit(Collider2D collider, Vector2 hitPosition)
    {
        bool validHit = false;

        // check collider transform to see if target should be hit
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

            foreach(Effect e in _activeStats.onHitEffects)
            {
                e.Apply(c.gameObject);
            }
        }

        // to do
        // play sound
        // play animation or effect
    }

    public void Equip()
    {
        Debug.Log("Weapon equipped");
        GameObject g = GameObject.Instantiate(_data.prefab) as GameObject;
        Transform = g.transform;
        
        _projectileSpawner = Transform.GetComponent<ProjectileSpawner>();
        if(_projectileSpawner != null) _projectileSpawner.Initialize(this);

        _reloadManager = Transform.GetComponent<ReloadManager>();
        _reloadManager.Initialize(this);

        Transform.parent = Owner.transform.Find("WeaponParent");

        _equipped = true;
    }

    public void Unequip()
    {
        GameObject.Destroy(Transform.gameObject);
        Transform = null;
        _projectileSpawner = null;
        _reloadManager = null;

        _equipped = false;
    }
}
