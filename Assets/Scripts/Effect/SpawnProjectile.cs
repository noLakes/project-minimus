using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnProjectile : Effect
{
    public GameObject projectilePrefab;
    public LayerMask hitDetectionMask;
    public List<Effect> onHitEffects;
    private Transform _projectileSource;

    public override void Apply(EffectArgs args)
    {
        _projectileSource = args.Source;
        var go = Instantiate(projectilePrefab, args.SourcePoint, quaternion.identity);
        var projectile = go.GetComponent<Projectile>();
        projectile.Initialize(Utility.GetDirection2D(args.SourcePoint, args.EffectPoint), ProcessHit);
    }
    
    private bool ProcessHit(Collider2D collider, Vector2 hitPoint, Vector2 origin)
    {
        if (collider.transform == _projectileSource) return false;
        
        var validHit = false;

        // check collider Transform to see if target should be hit
        // FIX THIS PLZ!!!
        if (collider.transform.tag == "Obstacle") validHit = true;
        else if (collider.transform.tag == "Character") validHit = true;

        if(validHit) OnHit(collider, hitPoint, origin);
        
        return validHit;
    }

    private void OnHit(Collider2D collider, Vector2 hitPosition, Vector2 origin)
    {
        if (!collider.transform.TryGetComponent<CharacterManager>(out var cm)) return;
        cm.ReceiveHit(_projectileSource, origin);
        
        if (onHitEffects.Count > 0 && cm != null)
        {
            Debug.Log("Weapon applying own on hit effects. Count: " + onHitEffects.Count);
            var wepEffectArgs = new EffectArgs(_projectileSource, cm.transform, hitPosition);
            Effect.ApplyEffectList(onHitEffects, wepEffectArgs);
        }

        // play sound
        // play animation or particle effects
    }
}
