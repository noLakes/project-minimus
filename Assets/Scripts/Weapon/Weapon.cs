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
    
    public Transform transform { get; private set; }
    ProjectileSpawner _projectileSpawner;
    ReloadManager _reloadManager;

    bool _equipped;
    public bool Equipped { get => _equipped; } 

    public Character owner { get; protected set; }

    public Weapon(WeaponData initialData, Character owner)
    {
        uid = System.Guid.NewGuid().ToString();
        _data = initialData;
        _baseStats = new WeaponStats(initialData);
        _activeStats = _baseStats;
        this.owner = owner;
    }

    public void Attack(Vector2 attackLocation)
    {
        if(_data.type == WeaponType.Melee) // melee attack
        {
            
        }
        else // ranged attack
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

        if(validHit) OnHit(collider, hitPosition);
        
        return validHit;
    }

    public void OnHit(Collider2D collider, Vector2 hitPosition)
    {
        // apply hit actions
        // to do
    }

    public void Equip()
    {
        Debug.Log("Weapon equipped");
        GameObject g = GameObject.Instantiate(_data.prefab) as GameObject;
        transform = g.transform;

        _projectileSpawner = transform.GetComponent<ProjectileSpawner>();
        _projectileSpawner.Initialize(this);

        _reloadManager = transform.GetComponent<ReloadManager>();
        _reloadManager.Initialize(this);

        transform.parent = owner.transform.Find("WeaponParent");

        _equipped = true;
    }

    public void Unequip()
    {
        GameObject.Destroy(transform.gameObject);
        transform = null;
        _projectileSpawner = null;
        _reloadManager = null;

        _equipped = false;
    }
}
