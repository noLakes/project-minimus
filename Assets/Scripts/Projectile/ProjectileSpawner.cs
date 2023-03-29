using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    protected GameObject _projectilePrefab;

    [SerializeField]
    protected Transform spawnPoint;

    protected Weapon _weapon;

    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
    }
    
    public virtual void Spawn(Vector2 shootDir, Quaternion rotation)
    {
        // add default behavior?
    }
}
