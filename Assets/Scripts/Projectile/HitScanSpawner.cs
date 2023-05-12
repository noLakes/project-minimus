using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanSpawner : ProjectileSpawner
{
    //[SerializeField] private GameObject tracerPrefab;

    [SerializeField] private LayerMask layerMask;
    
    public override void Spawn(Vector2 shootPoint)
    {
        Vector2 origin = spawnPoint.position;
        Vector2 shootDir = (shootPoint - origin).normalized;
        
        RaycastHit2D ray = Physics2D.Raycast(origin, shootDir, Weapon.Stats.Range, layerMask);

        DrawTracer(origin, shootPoint);

        if (ray.collider == null) return;
        
        // hit
        Debug.Log("Hit: " + ray.collider.name);
        Debug.DrawLine(origin, ray.point, Color.cyan, 2f);
        Weapon.ProcessHit(ray.collider, ray.point);
    }

    public override void Spawn()
    {
        Vector2 origin = spawnPoint.position;
        Vector2 dir = transform.parent.right;

        RaycastHit2D ray = Physics2D.Raycast(origin, dir, Weapon.Stats.Range, layerMask);

        DrawTracer(origin, dir);

        if (ray.collider == null) return;
        
        // hit
        Debug.Log("Hit: " + ray.collider.name);
        Debug.DrawLine(origin, ray.point, Color.cyan, 2f);
        Weapon.ProcessHit(ray.collider, ray.point);
    }
    
    private void DrawTracer(Vector2 origin, Vector2 dir)
    {
        // implement after making tracer prefabs
        Debug.DrawLine(
            origin,
            origin + (dir * Weapon.Stats.Range),
                Color.red, 2f);
    }
}
