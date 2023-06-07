using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanSpawner : ProjectileSpawner
{
    [SerializeField] private TrailRenderer _trailRendererPrefab;
    
    public override void Spawn(Vector2 shootPoint)
    {
        Vector2 origin = spawnPoint.position;
        Vector2 shootDir = (shootPoint - origin).normalized;
        
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

    public override void Spawn()
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
        // implement after making tracer prefabs
        var trail = Instantiate(_trailRendererPrefab, (Vector2)transform.position, Quaternion.identity);
        StartCoroutine(CastTracerRoutine(trail, endPos));
    }
    
    private IEnumerator CastTracerRoutine(TrailRenderer trail, Vector2 endPos)
    {
        yield return null;
        trail.transform.position = endPos;
    }
}
