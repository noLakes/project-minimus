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
    
    public virtual void Spawn(Vector2 shootPoint, Quaternion rotation)
    {
        // do nothing by default
    }
}
