using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    Character _character;
    public Character Character { get => _character; }

    Weapon _currentWeapon;
    Weapon CurrentWeapon { get => _currentWeapon; }
    
    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    public Transform projectileSpawnPoint;

    public void Initialize(Character character)
    {
        _character = character;
    }

    void Update()
    {
        //Vector2 mousePosition = Utility.GetMouseWorldPosition2D();
        //Debug.DrawLine(transform.position, mousePosition, Color.red);
    }

    public void Attack(Vector2 location)
    {
        //attack with character active weapon;
        _currentWeapon.Attack(location);
    }

    /*
    public void Shoot(Vector2 shootPoint)
    {
        Vector2 shootDir = (shootPoint - (Vector2)transform.position).normalized;
        RigidBodyProjectile.Spawn(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity, shootDir);
    }
    */

    public void EquipWeapon(Weapon weapon)
    {
        if(_currentWeapon != null && weapon != _currentWeapon) _currentWeapon.Unequip();

        _currentWeapon = weapon;
        _currentWeapon.Equip();
        projectileSpawnPoint = _currentWeapon.transform.Find("weaponEnd");
        
    }

    public void AddWeapon(Weapon newWeapon)
    {
        _character.AddWeapon(newWeapon);
    }
}
