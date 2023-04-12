using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyProjectile : Projectile
{
    public float speed; // Speed of projectile.

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
        //Debug.Log("projectile initialized towards: " + moveDirection);
    }

    void OnEnable()
    {
        _origin = _currentPosition = transform.position;
        // replace later with bounds check?
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (Game.Instance.gameIsPaused) return;
        
        HandleRangeCheck();
        CheckFlybyCollision();
        
        _lastPosition = _currentPosition;
        _currentPosition += _moveDirection * speed * Time.deltaTime;

        // Move ourselves towards the target position at every frame.

        _distanceTravelled += speed * Time.deltaTime; // Record the distance we are travelling.

        // Set our position to <currentPosition>.
        transform.position = _currentPosition;
        float totalDistance = Vector2.Distance(_origin, _moveDirection);

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
        if (speed < 12f) return; // too slow to warrant checking
        
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
    
    public static RigidBodyProjectile Spawn(GameObject prefab, Vector2 startPos, Vector2 shootDir, Quaternion rotation)
    {
        // Spawn a GameObject based on a prefab, and returns its Projectile component.
        GameObject go = Instantiate(prefab, startPos, rotation);
        RigidBodyProjectile p = go.GetComponent<RigidBodyProjectile>();

        // Rightfully, we should throw an error here instead of fixing the error for the user. 
        if (!p) p = go.AddComponent<RigidBodyProjectile>();

        // Initialize the projectile so that it can work.
        p.Initialize(shootDir);

        return p;
    }
    
}