using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyProjectile : MonoBehaviour
{
    public float speed = 8.5f; // Speed of projectile.

    private Vector2 _moveDirection;
    public Vector2 MoveDirection
    {
        get => _moveDirection;
        set => _moveDirection = value;
    }

    bool isHoming = false;
    Transform homingTarget; // Who we are homing at.
    float aoeRadius;

    //public List<Affect> onHitAffects;
    //private AbilityData sourceAbility = null;
    //private UnitManager caster = null;

    Vector2 currentPosition; // Store the current position we are at.
    float distanceTravelled; // Record the distance travelled.

    //public float arcFactor = 0.5f; // Higher number means bigger arc.
    Vector2 origin; // To store where the projectile first spawned.

    void OnEnable()
    {
        origin = currentPosition = transform.position;
        if (homingTarget)
        {
            _moveDirection = homingTarget.position;
            isHoming = true;
        }

        // replace later with bounds check?
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (Game.Instance.gameIsPaused) return;

        if (isHoming)
        {
            if (!homingTarget)
            {
                Destroy(gameObject);
                return; // Stops executing this function.
            }

            _moveDirection = homingTarget.position;
            currentPosition += _moveDirection.normalized * speed * Time.deltaTime;
        }
        else currentPosition += _moveDirection * speed * Time.deltaTime;

        // Move ourselves towards the target position at every frame.

        distanceTravelled += speed * Time.deltaTime; // Record the distance we are travelling.

        // Set our position to <currentPosition>.
        transform.position = currentPosition;
        float totalDistance = Vector2.Distance(origin, _moveDirection);

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

    // So that other scripts can use Projectile.Spawn to spawn a projectile.
    public static RigidBodyProjectile Spawn(GameObject prefab, Vector2 position, Quaternion rotation, Transform homingTarget, float aoeRadius = 0f)
    {
        // Spawn a GameObject based on a prefab, and returns its Projectile component.
        GameObject go = Instantiate(prefab, position, rotation);
        RigidBodyProjectile p = go.GetComponent<RigidBodyProjectile>();

        // Rightfully, we should throw an error here instead of fixing the error for the user. 
        if (!p) p = go.AddComponent<RigidBodyProjectile>();

        // Set the projectile's target, so that it can work.
        //p.onHitAffects = onHitAffects;
        p.homingTarget = homingTarget;
        p.aoeRadius = aoeRadius;

        return p;
    }

    public static RigidBodyProjectile Spawn(GameObject prefab, Vector2 position, Quaternion rotation, Vector2 shootDir, float aoeRadius = 0f)
    {
        // Spawn a GameObject based on a prefab, and returns its Projectile component.
        GameObject go = Instantiate(prefab, position, rotation);
        RigidBodyProjectile p = go.GetComponent<RigidBodyProjectile>();

        // Rightfully, we should throw an error here instead of fixing the error for the user. 
        if (!p) p = go.AddComponent<RigidBodyProjectile>();

        // Set the projectile's target, so that it can work.
        //p.onHitAffects = onHitAffects;
        p.MoveDirection = shootDir;
        p.aoeRadius = aoeRadius;

        return p;
    }
}