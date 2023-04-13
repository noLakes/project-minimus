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
        
        //Debug.Log(_rigidbody2D.velocity);
        /*
        if (_rigidbody2D.velocity.x < 0.5f || _rigidbody2D.velocity.y < 0.5f)
        {
            _rigidbody2D.drag *= 10;
            enabled = false;
        }
        */

        _lastPosition = _currentPosition;
        _currentPosition = transform.position;

        _distanceTravelled = Vector2.Distance(_currentPosition, _origin);
        
        // rotates toward move direction
        transform.up = _moveDirection;
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
        if(CurrentHitCount >= maxHitCount) Destroy(gameObject);
    }

    private void CheckFlybyCollision()
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
    private void HandleRangeCheck()
    {
        if (_linkedWeapon.Stats.range > 0f && _distanceTravelled >= _linkedWeapon.Stats.range)
        {
            Debug.Log(name + " destroyed at max range");
            Destroy(gameObject);
        }
    }
    
    public static PhysicsProjectile Spawn(GameObject prefab, Vector2 startPos, Vector2 shootDir, Quaternion rotation)
    {
        // Spawn a GameObject based on a prefab, and returns its Projectile component.
        GameObject go = Instantiate(prefab, startPos, rotation);
        PhysicsProjectile p = go.GetComponent<PhysicsProjectile>();

        // Rightfully, we should throw an error here instead of fixing the error for the user. 
        if (!p) p = go.AddComponent<PhysicsProjectile>();

        // Initialize the projectile so that it can work.
        p.Initialize(shootDir);

        return p;
    }
    
}