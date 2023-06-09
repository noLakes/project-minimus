using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HitScanSpawner : ProjectileSpawner
{
    [FormerlySerializedAs("_trailRendererPrefab")] [SerializeField] private TrailRenderer trailRendererPrefab;
    
    public override void Trigger(Vector2 shootPoint)
    {
        Vector2 origin = spawnPoint.position;
        Vector2 shootDir = Utility.GetDirection2D(origin, shootPoint);
        
        RaycastHit2D ray = Physics2D.Raycast(origin, shootDir, Weapon.Stats.range, Utility.GetFactionLayerMask(Weapon.Owner));
        
        if (ray.collider == null)
        {
            DrawTracer(origin + (shootDir * Weapon.Stats.range));
            return;
        }
        
        // hit
        Debug.Log("Hit: " + ray.collider.name);
        //Debug.DrawLine(origin, ray.point, Color.cyan, 2f);
        DrawTracer(ray.point);
        Weapon.ProcessHit(ray.collider, ray.point, origin);
    }

    public override void Trigger()
    {
        Vector2 origin = spawnPoint.position;
        Vector2 dir = transform.parent.right;

        RaycastHit2D ray = Physics2D.Raycast(origin, dir, Weapon.Stats.range, Utility.GetFactionLayerMask(Weapon.Owner));
        

        if (ray.collider == null)
        {
            DrawTracer(origin + (dir * Weapon.Stats.range));
            return;
        }
        
        // hit
        Debug.Log("Hit: " + ray.collider.name);
        //Debug.DrawLine(origin, ray.point, Color.cyan, 2f);
        DrawTracer(ray.point);
        Weapon.ProcessHit(ray.collider, ray.point, origin);
    }
    
    private void DrawTracer(Vector2 endPos)
    {
        var trail = Instantiate(trailRendererPrefab, (Vector2)transform.position, Quaternion.identity);
        StartCoroutine(CastTracerRoutine(trail, endPos));
    }
    
    private IEnumerator CastTracerRoutine(TrailRenderer trail, Vector2 endPos)
    {
        // will need some more work when advanced tracers are desired
        yield return null;
        trail.transform.position = endPos;
    }

    public override ProjectileType SpawnedType => ProjectileType.HitScan;
}
