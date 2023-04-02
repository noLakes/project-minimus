using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotSpawner : ProjectileSpawner
{
    public override void Spawn(Vector2 shootLocation, Quaternion rotation)
    {
        Vector2 shootDir = (shootLocation - (Vector2)transform.position).normalized;
        GameObject go = Instantiate(_projectilePrefab, spawnPoint.position, rotation);
        
        Projectile p = go.GetComponent<Projectile>();
        
        p.Initialize(shootDir);
        p.LinkWeapon(_weapon);
    }
}
