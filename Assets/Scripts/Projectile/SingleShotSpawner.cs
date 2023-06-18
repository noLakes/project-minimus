using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotSpawner : ProjectileSpawner
{
    public override void Spawn(Vector2 shootPoint)
    {
        Vector2 shootDir = Utility.GetDirection2D((Vector2)transform.position, shootPoint);
        var go = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        
        var projectile = go.GetComponent<Projectile>();
        
        projectile.Initialize(shootDir, Weapon.ProcessHit);
    }
    
    public override void Spawn()
    {
        var go = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        
        var projectile = go.GetComponent<Projectile>();

        projectile.Initialize(transform.parent.right, Weapon.ProcessHit); // parent.right represents where the weapon is pointed
    }
}
