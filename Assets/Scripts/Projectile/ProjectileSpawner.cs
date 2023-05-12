using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform spawnPoint;
    protected Weapon Weapon;

    public void Initialize(Weapon weapon)
    {
        Weapon = weapon;
    }
    
    public virtual void Spawn(Vector2 shootPoint)
    {
        // do nothing by default
    }
    
    // spawn without direction will just shoot toward weapon forward
    public virtual void Spawn()
    {
        // do nothing by default
    }
}
