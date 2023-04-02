using System;
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
    public Transform projectileSpawnPoint;
    [SerializeField]
    private Transform weaponParent;
    private AimWeapon _aimWeapon;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void Initialize(Character character)
    {
        _character = character;
    }

    private void Awake()
    {
        weaponParent = transform.Find("WeaponParent");
        _aimWeapon = weaponParent.GetComponent<AimWeapon>();
    }

    void Update()
    {

    }

    public void Damage(int amount)
    {
        _character.health -= amount;
        Debug.Log(transform.name + " took " + amount + " damage. New health: " + _character.health + "/" + _character.maxHealth);
        if(_character.health <= 0) Die();
    }

    public void Heal(int amount)
    {
        int currentHealth = _character.health;
        _character.health = Mathf.Min(currentHealth, _character.maxHealth);
    }

    private void Die()
    {
        Debug.Log(transform.name + " is dead.");
        // update _character if needed
        Destroy(transform.gameObject);
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
        projectileSpawnPoint = _currentWeapon.Transform.Find("weaponEnd");
        
        _aimWeapon.UpdateSpriteRenderers(
            _currentWeapon.Transform.Find("Sprite").GetComponent<SpriteRenderer>(),
            spriteRenderer
            );
    }

    public Weapon AddWeapon(Weapon newWeapon)
    {
        _character.AddWeapon(newWeapon);

        return newWeapon;
    }
}
