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

    }

    public void Attack(Vector2 location)
    {
        //attack with character active weapon;
        _currentWeapon.Attack(location);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if(_currentWeapon != null && weapon != _currentWeapon) _currentWeapon.Unequip();

        _currentWeapon = weapon;
        _currentWeapon.Equip();
        projectileSpawnPoint = _currentWeapon.transform.Find("weaponEnd");
    }

    public Weapon AddWeapon(Weapon newWeapon)
    {
        _character.AddWeapon(newWeapon);

        return newWeapon;
    }
}
