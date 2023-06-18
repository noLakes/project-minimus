using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    Physics,
    Regular,
    HitScan
}

public abstract class Projectile : MonoBehaviour
{
    [HideInInspector] public ProjectileType Type;
    public int maxHitCount;
    protected int CurrentHitCount;
    public bool persistAfterHit;
    public bool attachAfterHit;
    public bool persistAfterStop;
    protected float Range;

    public delegate bool ProcessHitDelegate(Collider2D collider, Vector2 hitPosition, Vector2 origin);
    protected ProcessHitDelegate MyProcessHitDelegate;

    public abstract void Initialize(Vector2 shootDirection, ProcessHitDelegate hitDelegate);

    protected abstract void OnHit(Collider2D other, Vector2 hitPoint);

    protected abstract void Stop();

    public void SetRange(float range) => Range = range;
}
