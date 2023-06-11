using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnProjectile : Effect
{
    public GameObject projectilePrefab;
    public override void Apply(EffectArgs args)
    {
        var go = Instantiate(projectilePrefab, args.SourcePoint, quaternion.identity);
        var projectile = go.GetComponent<Projectile>();
        //projectile.LinkWeapon();
        projectile.Initialize(Utility.GetDirection2D(args.SourcePoint, args.EffectPoint));
    }
}
