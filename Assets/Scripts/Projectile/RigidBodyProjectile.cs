using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyProjectile : Projectile
{
    public float speed; // Speed of projectile.
    private Vector2 _moveDirection;
    private Vector2 _lastPosition; // Store last position for backward raycast collision checks
    private Vector2 _currentPosition; // Store the current position we are at.
    private float _distanceTravelled; // Record the distance travelled.
    private Vector2 _origin; // To store where the projectile first spawned.
    private Rigidbody2D _rb;
    private Collider2D _collider;

    public override void Initialize(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
        CurrentHitCount = 0;
        _currentPosition = transform.position;
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
        
        HandleRangeCheck();
        HandleFlybyCollision();

        _lastPosition = _currentPosition;
        _currentPosition += _moveDirection * (speed * Time.deltaTime);

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
            // stick to target
            transform.parent = other.transform;
        }
        
        Stop();
    }
    
    private void HandleFlybyCollision()
    {
        if (speed < 12f) return; // too slow to warrant checking
        
        RaycastHit2D ray = Physics2D.Raycast(_lastPosition, _moveDirection, _distanceTravelled);
        
        if (!ray.collider) return;
        
        if (_linkedWeapon.ProcessHit(ray.collider, ray.point))
        {
            OnHit(ray.collider);
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
            Debug.Log("Turning off projectile script");
            _rb.velocity = Vector2.zero;
            _collider.isTrigger = false;
            enabled = false;
        }
        else
        {
            Debug.Log("Destroying stopped projectile");
            Destroy(gameObject);
        }
    }
    
}