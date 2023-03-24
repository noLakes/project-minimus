using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    GameObject _projectilePrefab;

    public GameObject ProjectilePrefab
    {
        get => _projectilePrefab;
        set => _projectilePrefab = value;
    }

    protected override void Attack(Vector2 direction)
    {
        base.Attack(direction);

        // shoot projectile ---- needs to work regardless of projectile type
    }
}
