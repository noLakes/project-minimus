using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanSpawner : ProjectileSpawner
{
    private TrailRenderer _trailRenderer;

    private void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    public override void Spawn(Vector2 shootPoint)
    {
        Vector2 origin = spawnPoint.position;
        Vector2 shootDir = (shootPoint - origin).normalized;
        
        RaycastHit2D ray = Physics2D.Raycast(origin, shootDir, Weapon.Stats.Range, Weapon.GetFactionLayerMask());

        DrawTracer(origin, shootPoint);

        if (ray.collider == null) return;
        
        // hit
        Debug.Log("Hit: " + ray.collider.name);
        Debug.DrawLine(origin, ray.point, Color.cyan, 2f);
        Weapon.ProcessHit(ray.collider, ray.point, origin);
    }

    public override void Spawn()
    {
        Vector2 origin = spawnPoint.position;
        Vector2 dir = transform.parent.right;

        RaycastHit2D ray = Physics2D.Raycast(origin, dir, Weapon.Stats.Range, Weapon.GetFactionLayerMask());
        

        if (ray.collider == null)
        {
            DrawTracer(origin, origin + (dir * Weapon.Stats.Range));
            return;
        }
        
        // hit
        Debug.Log("Hit: " + ray.collider.name);
        Debug.DrawLine(origin, ray.point, Color.cyan, 2f);
        DrawTracer(origin, ray.point);
        Weapon.ProcessHit(ray.collider, ray.point, origin);
    }
    
    private void DrawTracer(Vector2 origin, Vector2 end)
    {
        // implement after making tracer prefabs
    }
}
