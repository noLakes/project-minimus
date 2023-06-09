using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyProjectile : Projectile
{
    public float speed; // Speed of projectile.
    private Vector2 _moveDirection;
    private Vector2 _currentPosition; // Store the current position we are at.
    private float _distanceTravelled; // Record the distance travelled.
    private Vector2 _origin; // To store where the projectile first spawned.
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private bool _attachedToTarget;

    public override void Initialize(Vector2 shootDirection, ProcessHitDelegate hitDelegate, Transform source = null)
    {
        type = ProjectileType.Regular;
        _moveDirection = shootDirection;
        CurrentHitCount = 0;
        _currentPosition = transform.position;
        MyProcessHitDelegate = hitDelegate;
    }

    private void OnEnable()
    {
        _origin = _currentPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Game.Instance.GameIsPaused) return;
        
        _currentPosition += _moveDirection * (speed * Time.deltaTime);
        
        HandleLifetime();
        HandleRangeCheck();
        HandleFlybyCollision();

        // Move ourselves towards the target position at every frame.
        _distanceTravelled += speed * Time.deltaTime; // Record the distance we are travelling.

        // Set our position to <currentPosition>.
        transform.position = _currentPosition;
        // rotates toward move direction
        transform.up = _moveDirection;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if hit collider belongs to hittable target....
        if (MyProcessHitDelegate.Invoke(other, transform.position, _origin))
        {
            OnHit(other, GetAccurateHitPosition(other));
        }
    }

    protected override void OnHit(Collider2D other, Vector2 hitPoint)
    {
        CurrentHitCount++;
        if (CurrentHitCount < maxHitCount)
        {
            return;
        }

        if (!persistAfterHit)
        {
            Destroy();
        }

        if (attachAfterHit)
        {
            // stick to target
            transform.parent = other.transform;
            _attachedToTarget = true;
        }
        
        Stop();
    }
    
    private Vector2 GetAccurateHitPosition(Collider2D other)
    {
        GameObject otherGo = other.gameObject;
        int initialLayer = otherGo.layer;
        otherGo.layer = 31; // reserved single object layer;
        RaycastHit2D hit = Physics2D.Raycast(_origin, _moveDirection, 1000f, Game.Instance.SingleObjectReservedMask);
        //Debug.DrawLine(_origin, hit.point, new Color(0, 1, 0, 0.5f), 5f);
        if (hit.point == Vector2.zero) Debug.Log("HIT ZERO ISSUE!");
        other.gameObject.layer = initialLayer;
        return hit.point;
    }

    private void HandleLifetime()
    {
        if (lifetime == 0f) return;

        LifetimeElapsed += Time.deltaTime;

        if(LifetimeElapsed >= lifetime) Destroy();
    }
    
    private void HandleFlybyCollision()
    {
        if (speed < 8f) return; // too slow to warrant checking
        
        RaycastHit2D ray = Physics2D.Raycast(_currentPosition, _moveDirection, 1f);
        
        if (!ray.collider) return;
        
        if (MyProcessHitDelegate.Invoke(ray.collider, ray.point, _origin))
        {
            OnHit(ray.collider, ray.point);
        }
    }

    private void HandleRangeCheck()
    {
        if (Range > 0f && _distanceTravelled >= Range)
        {
            Stop();
        }
    }
    
    protected override void Stop()
    {
        if (persistAfterStop)
        {
            _rb.velocity = Vector2.zero;
            stopped = true;
            if(!activeAfterStop)_collider.isTrigger = false;
            
            if (_attachedToTarget)
            {
                Destroy(_rb);
                _rb = null;
                Destroy(_collider);
                _collider = null;
            }
            
            Debug.Log("Projectile range: " + _distanceTravelled);
            if(!activeAfterStop) enabled = false;
        }
        else
        {
            Destroy();
        }
    }
}