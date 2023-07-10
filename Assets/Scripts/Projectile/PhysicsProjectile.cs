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
        //HandleFlybyCollision();

        _distanceTravelled = Vector2.Distance(_currentPosition, _origin);

        // rotates toward move direction
        transform.up = _moveDirection;
    }

    private void FixedUpdate()
    {
        if (Game.Instance.GameIsPaused) return;
        //CheckCollision();
        UpdateColliderSize();
        HandleVelocityCheck();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if hit collider belongs to hittable target....
        if (MyProcessHitDelegate.Invoke(other, transform.position, _origin))
        {
            //OnHit(other, other.ClosestPoint(other.ClosestPoint(transform.TransformPoint(_collider.bounds.center))));
            //OnHit(other, other.ClosestPoint(transform.position));
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
        int initialLayer = other.gameObject.layer;
        other.gameObject.layer = 31; // reserved single object layer;
        LayerMask layerMask = 1 << 31;
        RaycastHit2D hit = Physics2D.Raycast(_origin, _moveDirection, 1000f, layerMask);
        Debug.DrawLine(_origin, hit.point, new Color(0, 1, 0, 0.7f), 5f);
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
    
    // might be able to clear...
    private void HandleFlybyCollision()
    {
        if (velocity < 5f) return; // too slow to warrant checking

        // old method that raycasts backwards
        //RaycastHit2D ray = Physics2D.Raycast(_currentPosition, _moveDirection*-1f, 1f);
        
        RaycastHit2D ray = Physics2D.Raycast((_currentPosition), _moveDirection, _rb.velocity.magnitude * Time.fixedDeltaTime);

        if (!ray.collider || ray.collider == _collider)
        {
            Debug.DrawLine(_currentPosition, _currentPosition + (_rb.velocity * Time.fixedDeltaTime), Color.yellow, 3f);
            return;
        }

        Debug.DrawLine(_currentPosition, ray.point, Color.green, 3f);
        
        if (MyProcessHitDelegate.Invoke(ray.collider, ray.point, _origin))
        {
            OnHit(ray.collider, ray.point);
        }
    }
    
    // might be able to clear
    private void CheckCollision()
    {
        //if (velocity < 8f) return; // too slow to warrant checking
        //DrawColliderPosition();

        float dist = (_rb.velocity * Time.fixedDeltaTime).magnitude;
        RaycastHit2D rayFwd = Physics2D.Raycast(_currentPosition, _moveDirection, dist);
        RaycastHit2D rayBkwd = Physics2D.Raycast(_currentPosition, -_moveDirection, dist);

        float opacity = 1f;
        Color miss = new Color(1, 0.92f, 0.016f, opacity);
        Color hit = new Color(0, 1, 0, opacity);
        
        if (!rayFwd.collider || rayFwd.collider == _collider)
        {
            Debug.DrawLine(_currentPosition, _currentPosition + (_rb.velocity * Time.fixedDeltaTime), miss, 5f);
            return;
        }

        if (MyProcessHitDelegate.Invoke(rayFwd.collider, rayFwd.point, _origin))
        {
            Debug.DrawLine(_currentPosition, rayFwd.point, hit, 5f);
            OnHit(rayFwd.collider, rayFwd.point);
            return;
        }
        
        if (!rayBkwd.collider || rayBkwd.collider == _collider)
        {
            Debug.DrawLine(_currentPosition, _currentPosition - (_rb.velocity * Time.fixedDeltaTime), miss, 5f);
            return;
        }

        if (MyProcessHitDelegate.Invoke(rayBkwd.collider, rayBkwd.point, _origin))
        {
            Debug.DrawLine(_currentPosition, rayBkwd.point, hit, 5f);
            OnHit(rayBkwd.collider, rayBkwd.point);
        }
    }

    private void UpdateColliderSize()
    {
        if (!_adjustColliderSizeFlag)
        {
            _adjustColliderSizeFlag = true;
            return;
        }
        
        Vector2 velocity = _rb.velocity * Time.fixedDeltaTime;
        Vector2 colliderIncrease = transform.InverseTransformVector(velocity);
        _collider.size = _initialColliderSize + colliderIncrease;
        _collider.offset = new Vector2(0, -(colliderIncrease.y / 4f));
    }

    private void DrawColliderPosition()
    {
        var boxCollider = (BoxCollider2D)_collider;
        var size = boxCollider.bounds.extents.y;

        Vector2 pos = transform.position;

        Debug.DrawLine(pos, pos + (_moveDirection * size), Color.cyan, 5f);
        Debug.DrawLine(pos, pos + -(_moveDirection * size), Color.cyan, 5f);
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