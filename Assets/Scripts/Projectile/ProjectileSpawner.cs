using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    protected GameObject _projectilePrefab;
    Weapon _weapon;

    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
    }
    
    public virtual void Spawn(Vector2 position, Quaternion rotation, Vector2 shootDir)
    {
        // add default behavior?
    }
}
