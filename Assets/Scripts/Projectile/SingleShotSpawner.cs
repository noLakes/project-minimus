using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotSpawner : ProjectileSpawner
{
    public override void Spawn(Vector2 position, Quaternion rotation, Vector2 shootDir)
    {
        GameObject go = Instantiate(_projectilePrefab, position, rotation);
    }
}
