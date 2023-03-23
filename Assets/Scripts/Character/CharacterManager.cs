using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private Character _character;
    public Character Character { get => _character; }

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    private Transform projectileSpawnPoint;

    public void Initialize(Character character)
    {
        _character = character;
        character.Activate();
    }

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
        RigidBodyProjectile.Spawn(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity, shootDir);
    }

    public void Shoot(Transform target)
    {
        RigidBodyProjectile.Spawn(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity, target);
    }

    public void ChangeMaxHealth(int amount, bool adjustCurrentHealth = true)
    {
        _character.maxHealth += amount;

        if (adjustCurrentHealth)
        {
            if (_character.health + amount <= 0)
            {
                _character.health = 1;
            }
            else
            {
                _character.health += amount;
            }
        }

        // update health UI
    }
}
