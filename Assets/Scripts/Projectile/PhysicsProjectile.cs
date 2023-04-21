using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsProjectile : Projectile
{
    public float velocity; //  used to launch projectile.
    private Rigidbody2D _rigidbody2D;

    Vector2 _moveDirection;

    private Vector2 _lastPosition; // Store last position for backward raycast collision checks
    private Vector2 _currentPosition; // Store the current position we are at.
    private float _distanceTravelled; // Record the distance travelled.

    Vector2 _origin; // To store where the projectile first spawned.

    public override void Initialize(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
        CurrentHitCount = 0;
        _currentPosition = transform.position;
        Debug.Log("physics projectile initialized towards: " + moveDirection + " with force of " + velocity);
        _rigidbody2D.AddForce(_moveDirection * velocity, ForceMode2D.Impulse);
    }

    void OnEnable()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _origin = _currentPosition = transform.position;
    }

    void Update()
    {
        if (Game.Instance.gameIsPaused) return;

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
        if (_linkedWeapon.ValidateHit(other, transform.position))
        {
            OnHit();
        }
    }

    protected override void OnHit()
    {
        CurrentHitCount++;
        if (CurrentHitCount >= maxHitCount) Destroy(gameObject);
    }
    
    private void HandleFlybyCollision()
    {
        if (velocity < 12f) return; // too slow to warrant checking

        //Debug.DrawLine(_lastPosition, _currentPosition, Color.red);
        RaycastHit2D ray = Physics2D.Raycast(_lastPosition, _moveDirection, _distanceTravelled);

        if (ray.collider != null)
        {
            if (_linkedWeapon.ValidateHit(ray.collider, _lastPosition))
            {
                //Debug.DrawLine(_currentPosition, ray.point, Color.cyan, 5f);
                OnHit();
            }
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
        if (_linkedWeapon.Stats.range > 0f && _distanceTravelled >= _linkedWeapon.Stats.range)
        {
            Stop();
        }
    }

    protected override void Stop()
    {
        if (persistAfterStop)
        {
            Debug.Log("Turning off projectile script");
            _rigidbody2D.velocity = Vector2.zero;
            GetComponent<Collider2D>().isTrigger = false;
            enabled = false;
        }
        else
        {
            Debug.Log("Destroying stopped projectile");
            Destroy(gameObject);
        }
    }
}