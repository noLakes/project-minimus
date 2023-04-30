using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Weapon _linkedWeapon;
    public int maxHitCount;
    protected int CurrentHitCount;
    public bool persistAfterHit;
    public bool attachAfterHit;
    public bool persistAfterStop;

    public abstract void Initialize(Vector2 shootDirection);

    public virtual void LinkWeapon(Weapon linkedWeapon)
    {
        _linkedWeapon = linkedWeapon;
    }

    protected abstract void OnHit(Collider2D other, Vector2 hitPoint);

    protected abstract void Stop();
}
