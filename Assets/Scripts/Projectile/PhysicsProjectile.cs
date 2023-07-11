using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PhysicsProjectile : Projectile
{
    public float velocity; //  used to launch projectile.
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private Vector2 _moveDirection;
    private Vector2 _currentPosition; // Store the current position we are at.
    private float _distanceTravelled; // Record the distance travelled.
    private Vector2 _origin; // To store where the projectile first spawned.
    private bool _attachedToTarget;

    private bool _adjustColliderSizeFlag; // prevents first fixed update from extending box collider
    private Vector2 _initialColliderSize;

    public override void Initialize(Vector2 moveDirection, ProcessHitDelegate hitDelegate, Transform source = null)
    {
        type = ProjectileType.Physics;
        MyProcessHitDelegate = hitDelegate;
        _moveDirection = moveDirection;
        CurrentHitCount = 0;
        _origin = transform.position;
        _currentPosition = transform.position;
        _rb.AddForce(_moveDirection * velocity, ForceMode2D.Impulse);

        Selection.activeGameObject = gameObject;
        
        if (source == null) return;
        if (ignoreSourceCollision && source.TryGetComponent<Collider2D>(out var sourceCollider))
        {
            Physics2D.IgnoreCollision(_collider, sourceCollider);
        }
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _initialColliderSize = _collider.size;
        _origin = _currentPosition = transform.position;
    }

    private void Update()
    {
        if (Game.Instance.GameIsPaused) return;
        
        _currentPosition = transform.position;

        HandleLifetime();
        
        if (stopped) return;
        
        HandleRangeCheck();

        _distanceTravelled = Vector2.Distance(_currentPosition, _origin);

        // rotates toward move direction
        transform.up = _moveDirection;
    }

    private void FixedUpdate()
    {
        if (Game.Instance.GameIsPaused) return;
        if(_rb.velocity.magnitude > 5.0f) UpdateColliderSize();
        HandleVelocityCheck();
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
        if (CurrentHitCount < maxHitCount || maxHitCount == 0)
        {
            return;
        }

        if (!persistAfterHit)
        {
            Destroy();
        }

        if (attachAfterHit)
        {
            // attach to hit target
            transform.position = hitPoint;
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

    private void UpdateColliderSize()
    {
        if (!_adjustColliderSizeFlag)
        {
            _adjustColliderSizeFlag = true;
            return;
        }
        
        Vector2 velocity = _rb.velocity * Time.fixedDeltaTime;
        // velocity converted from world space distance to local space size for increase
        Vector2 colliderIncrease = transform.InverseTransformVector(velocity);
        _collider.size = _initialColliderSize + colliderIncrease;
        _collider.offset = new Vector2(0, -(colliderIncrease.y / 4f));
    }

    private void HandleVelocityCheck()
    {
        if (enabled && _distanceTravelled > 1f && _rb.velocity.magnitude < 0.5f)
        {
            Stop();
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
            _collider.size = _initialColliderSize;
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