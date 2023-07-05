using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ProjectileType
{
    Physics,
    Regular,
    HitScan
}

public abstract class Projectile : MonoBehaviour
{
    [FormerlySerializedAs("Type")] [HideInInspector] public ProjectileType type;
    [Min(0)] public int maxHitCount;
    protected int CurrentHitCount;
    [Min(0)] public float lifetime;
    protected float LifetimeElapsed;
    public bool ignoreSourceCollision;
    public bool persistAfterHit;
    public bool attachAfterHit;
    public bool persistAfterStop;
    public bool activeAfterStop;
    protected bool stopped;
    protected float Range;

    public delegate bool ProcessHitDelegate(Collider2D collider, Vector2 hitPosition, Vector2 origin);
    protected ProcessHitDelegate MyProcessHitDelegate;

    public delegate void DestructionDelegate(Vector2 location);
    protected DestructionDelegate MyDestructionDelegate;

    public abstract void Initialize(Vector2 shootDirection, ProcessHitDelegate hitDelegate, Transform source = null);

    protected abstract void OnHit(Collider2D other, Vector2 hitPoint);

    protected abstract void Stop();

    protected virtual void Destroy()
    {
        MyDestructionDelegate?.Invoke(transform.position);
        Destroy(gameObject);
    }

    public void SetDestructionDelegate(DestructionDelegate destructionDelegate)
    {
        MyDestructionDelegate = destructionDelegate;
    }

    public void SetRange(float range) => Range = range;
    
}
