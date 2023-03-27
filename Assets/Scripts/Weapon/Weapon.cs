using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    protected string uid;
    WeaponData _data;
    public WeaponData Data { get => _data; }
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
        this.owner = owner;
    }

    public void Attack(Vector2 attackLocation)
    {
        if(_data.type == WeaponType.Melee) // melee attack
        {
            
        }
        else // ranged attack
        {
            if(_reloadManager == null || _reloadManager.Ready)
            {
                // trigger attack
                //_projectileSpawner.Spawn()
                // notify ReloadManager
            }
        }
    }

    public void Equip()
    {
        GameObject g = GameObject.Instantiate(_data.prefab) as GameObject;
        transform = g.transform;
        _projectileSpawner = transform.GetComponent<ProjectileSpawner>();
        _reloadManager = transform.GetComponent<ReloadManager>();
        transform.parent = owner.transform.Find("WeaponParent");

        _equipped = true;
    }

    public void Unequip()
    {
        GameObject.Destroy(transform.gameObject);
        transform = null;
        _projectileSpawner = null;

        _equipped = false;
    }
}
