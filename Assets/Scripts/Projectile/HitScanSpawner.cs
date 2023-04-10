using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanSpawner : ProjectileSpawner
{
    //[SerializeField]
    //private GameObject tracerPrefab;

    [SerializeField] private LayerMask layerMask;
    public override void Spawn(Vector2 shootLocation, Quaternion rotation)
    {
        Vector2 origin = spawnPoint.position;
        Vector2 shootDir = (shootLocation - origin).normalized;
        
        RaycastHit2D ray = Physics2D.Raycast(origin, shootDir, 1000f, layerMask);

        DrawTracer(shootLocation);

        if (ray.collider != null)
        {
            // hit
            Debug.Log("Hit: " + ray.collider.name);
            Debug.DrawLine(origin, ray.point, Color.cyan, 2f);
            _weapon.ValidateHit(ray.collider, ray.point);
        }
    }

    private void DrawTracer(Vector2 shootLocation)
    {
        // implement after making tracer prefabs
        Debug.DrawLine(spawnPoint.position, shootLocation, Color.red, 2f);
    }
}
