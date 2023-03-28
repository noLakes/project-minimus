using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyProjectile : Projectile
{
    public float speed; // Speed of projectile.

    Vector2 _moveDirection;

    Vector2 _currentPosition; // Store the current position we are at.
    float _distanceTravelled; // Record the distance travelled.

    Vector2 _origin; // To store where the projectile first spawned.

    public override void Initialize(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
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

        // if true, trigger hit actions
        OnHit();
    }

    private void OnHit()
    {
        // trigger on hit actions
        Destroy(gameObject);
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