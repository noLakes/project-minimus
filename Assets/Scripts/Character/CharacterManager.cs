using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    private Transform projectileSpawnPoint;

    public void Initialize(Character character)
    {

    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 mousePosition = Utility.GetMouseWorldPosition2D();
        //Debug.DrawLine(transform.position, mousePosition, Color.red);
    }

    public void Attack(Vector2 location)
    {
        Shoot(location);
    }

    public void Attack(Transform target)
    {

    }

    public void Shoot(Vector2 shootPoint)
    {
        Vector2 shootDir = (shootPoint - (Vector2)transform.position).normalized;
        Projectile.Spawn(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity, shootDir);
    }

    public void Shoot(Transform target)
    {
        Projectile.Spawn(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity, target);
    }
}
