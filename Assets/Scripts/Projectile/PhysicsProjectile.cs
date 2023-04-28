using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsProjectile : Projectile
{
    public float velocity; //  used to launch projectile.
    private Rigidbody2D _rigidbody2D;
    private Vector2 _moveDirection;
    private Vector2 _lastPosition; // Store last position for backward raycast collision checks
    private Vector2 _currentPosition; // Store the current position we are at.
    private float _distanceTravelled; // Record the distance travelled.
    private Vector2 _origin; // To store where the projectile first spawned.

    public override void Initialize(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
        CurrentHitCount = 0;
        _currentPosition = transform.position;
        _rigidbody2D.AddForce(_moveDirection * velocity, ForceMode2D.Impulse);
    }

    private void OnEnable()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _origin = _currentPosition = transform.position;
    }

    private void Update()
    {
        if (Game.Instance.GameIsPaused) return;

        HandleRangeCheck();
        HandleFlybyCollision();
        
        _lastPosition = _currentPosition;
        _currentPosition = transform.position;

        _distanceTravelled = Vector2.Distance(_currentPosition, _origin);

        // rotates toward move direction
        transform.up = _moveDirection;
        
        HandleVelocityCheck();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if hit collider belongs to hittable target....
        if (_linkedWeapon.ProcessHit(other, transform.position))
        {
            OnHit(other);
        }
    }

    protected override void OnHit(Collider2D other)
    {
        CurrentHitCount++;
        if (CurrentHitCount < maxHitCount)
        {
            return;
        }

        if (!persistAfterHit)
        {
            Destroy(gameObject);
        }

        if (attachAfterHit)
        {
            // attach to hit target
            transform.parent = other.transform;
        }
        
        Stop();
    }
    
    private void HandleFlybyCollision()
    {
        if (velocity < 12f) return; // too slow to warrant checking
        
        RaycastHit2D ray = Physics2D.Raycast(_lastPosition, _moveDirection, _distanceTravelled);

        if (!ray.collider) return;
        
        if (_linkedWeapon.ProcessHit(ray.collider, ray.point))
        {
            OnHit(ray.collider);
        }
    }

    private void HandleVelocityCheck()
    {
        if (_distanceTravelled > 1f && _rigidbody2D.velocity.sqrMagnitude < Vector2.one.sqrMagnitude)
        {
            Stop();
        }
    }

    private void HandleRangeCheck()
    {
        if (_linkedWeapon.Stats.Range > 0f && _distanceTravelled >= _linkedWeapon.Stats.Range)
        {
            Stop();
        }
    }

    protected override void Stop()
    {
        if (persistAfterStop)
        {
            _rigidbody2D.velocity = Vector2.zero;
            GetComponent<Collider2D>().isTrigger = false;
            enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}