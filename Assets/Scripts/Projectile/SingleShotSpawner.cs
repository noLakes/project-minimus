using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotSpawner : ProjectileSpawner
{
    public override void Spawn(Vector2 shootPoint)
    {
        Vector2 shootDir = (shootPoint - (Vector2)transform.position).normalized;
        var go = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        
        var projectile = go.GetComponent<Projectile>();
        
        projectile.LinkWeapon(Weapon);
        projectile.Initialize(shootDir);
    }
    
    public override void Spawn()
    {
        var go = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        
        var projectile = go.GetComponent<Projectile>();
        
        projectile.LinkWeapon(Weapon);
        projectile.Initialize(transform.parent.right); // parent.right represents where the weapon is pointed
    }
}
