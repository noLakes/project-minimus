using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotSpawner : ProjectileSpawner
{
    public override void Spawn(Vector2 shootPoint, Quaternion rotation)
    {
        Vector2 shootDir = (shootPoint - (Vector2)transform.position).normalized;
        var go = Instantiate(projectilePrefab, spawnPoint.position, rotation);
        
        var projectile = go.GetComponent<Projectile>();
        
        projectile.Initialize(shootDir);
        projectile.LinkWeapon(Weapon);
    }
}
