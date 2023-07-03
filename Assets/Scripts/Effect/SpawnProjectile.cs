using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnProjectile", menuName = "Scriptable Objects/Effects/SpawnProjectile", order = 6)]
public class SpawnProjectile : Effect
{
    public GameObject projectilePrefab;
    public LayerMask hitDetectionMask;
    public List<Effect> onHitEffects;
    public List<Effect> onProjectileDestructionEffects;
    private Transform _projectileSource;

    public override void Trigger(EffectArgs args)
    {
        _projectileSource = args.Source;
        var go = Instantiate(projectilePrefab, args.SourcePoint, quaternion.identity);
        var projectile = go.GetComponent<Projectile>();
        projectile.Initialize(Utility.GetDirection2D(args.SourcePoint, args.EffectPoint), ProcessHit, args.Source);
        projectile.SetDestructionDelegate(OnSpawnedProjectileDestruction);
    }
    
    private bool ProcessHit(Collider2D collider, Vector2 hitPoint, Vector2 origin)
    {
        if (collider.transform == _projectileSource) return false;

        var validHit = false;

        // check other layer to see if target should be hit
        validHit = Utility.LayerMaskHasLayer(hitDetectionMask, collider.gameObject.layer);

        if(validHit) OnHit(collider.transform, hitPoint, origin);
        
        return validHit;
    }

    private void OnHit(Transform other, Vector2 hitPosition, Vector2 origin)
    {
        if (other.TryGetComponent<CharacterManager>(out var cm))
        {
            cm.ReceiveHit(_projectileSource, origin); // important for AI
        }

        if (onHitEffects.Count > 0)
        {
            Debug.Log("Weapon applying own on hit effects. Count: " + onHitEffects.Count);
            var wepEffectArgs = new EffectArgs(_projectileSource, other, hitPosition);
            TriggerEffectList(onHitEffects, wepEffectArgs);
        }

        // play sound
        // play animation or particle effects
    }
    
    private void OnSpawnedProjectileDestruction(Vector2 location)
    {
        Debug.Log("PROJECTILE DESTRUCTION DELEGATE TRIGGERED!");
        
        if (onProjectileDestructionEffects.Count == 0) return;

        var eArgs = new EffectArgs(_projectileSource, null, location);
        
        TriggerEffectList(onProjectileDestructionEffects, eArgs);
    }
}
