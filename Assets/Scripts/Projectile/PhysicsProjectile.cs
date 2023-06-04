using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsProjectile : Projectile
{
    public float velocity; //  used to launch projectile.
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Vector2 _moveDirection;
    private Vector2 _currentPosition; // Store the current position we are at.
    private float _distanceTravelled; // Record the distance travelled.
    private Vector2 _origin; // To store where the projectile first spawned.
    private bool _attachedToTarget;

    public override void Initialize(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
        CurrentHitCount = 0;
        _currentPosition = transform.position;
        _rb.AddForce(_moveDirection * velocity, ForceMode2D.Impulse);
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _origin = _currentPosition = transform.position;
    }

    private void Update()
    {
        if (Game.Instance.GameIsPaused) return;
        
        _currentPosition = transform.position;

        HandleRangeCheck();
        HandleFlybyCollision();

        _distanceTravelled = Vector2.Distance(_currentPosition, _origin);

        // rotates toward move direction
        transform.up = _moveDirection;
        
        HandleVelocityCheck();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if hit collider belongs to hittable target....
        if (_linkedWeapon.ProcessHit(other, transform.position, _origin))
        {
            OnHit(other, other.ClosestPoint(transform.position));
        }
    }

    protected override void OnHit(Collider2D other, Vector2 hitPoint)
    {
        CurrentHitCount++;
        if (CurrentHitCount < maxHitCount)
        {
            return;
        }

        //Debug.Log("Registering Hit");
        
        if (!persistAfterHit)
        {
            Destroy(gameObject);
        }

        if (attachAfterHit)
        {
            // attach to hit target
            transform.parent = other.transform;
            _attachedToTarget = true;
        }
        
        Stop();
    }
    
    private void HandleFlybyCollision()
    {
        if (velocity < 20f) return; // too slow to warrant checking
        
        RaycastHit2D ray = Physics2D.Raycast(_currentPosition, _moveDirection*-1f, 1f);

        if (!ray.collider) return;
        
        if (_linkedWeapon.ProcessHit(ray.collider, ray.point, _origin))
        {
            OnHit(ray.collider, ray.point);
        }
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
        if (_linkedWeapon.Stats.range > 0f && _distanceTravelled >= _linkedWeapon.Stats.range)
        {
            Stop();
        }
    }

    protected override void Stop()
    {
        if (persistAfterStop)
        {
            _rb.velocity = Vector2.zero;
            _collider.isTrigger = false;
            
            if (_attachedToTarget)
            {
                Destroy(_rb);
                _rb = null;
                Destroy(_collider);
                _collider = null;
            }
            
            Debug.Log("Projectile range: " + _distanceTravelled);
            enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}