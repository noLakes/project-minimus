using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private Character _character;
    private Weapon _currentWeapon;
    [SerializeField] private Transform weaponParent;
    private WeaponAimManager _weaponAimManager;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Initialize(Character character)
    {
        _character = character;
    }

    private void Awake()
    {
        weaponParent = transform.Find("WeaponParent");
        _weaponAimManager = weaponParent.GetComponent<WeaponAimManager>();
    }

    void Update()
    {

    }

    public void Damage(int amount)
    {
        _character.Health -= amount;
        Debug.Log(transform.name + " took " + amount + " damage.");
        if(_character.Health <= 0) Die();
    }

    public void Heal(int amount)
    {
        _character.Health += amount;
    }

    private void Die()
    {
        Debug.Log(transform.name + " is dead.");
        // update Game Instance CHARACTERS list if needed
        Destroy(transform.gameObject);
    }

    public void Attack(Vector2 location)
    {
        _currentWeapon.Attack(location);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if(_currentWeapon != null && weapon != _currentWeapon) _currentWeapon.Unequip();

        _currentWeapon = weapon;
        _currentWeapon.Equip(_character);
        
        _weaponAimManager.UpdateSpriteRenderers(
            _currentWeapon.SpriteRenderer,
            spriteRenderer
            );

        _weaponAimManager.SetWeapon(weapon);
    }

    public Weapon AddWeapon(Weapon newWeapon)
    {
        _character.AddWeapon(newWeapon);

        return newWeapon;
    }

    public Character Character
    {
        get => _character;
    }

    public Weapon CurrentWeapon
    {
        get => _currentWeapon;
    }
}
