using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private Character _character;
    private Weapon _currentWeapon;
    [SerializeField] private Transform weaponParent;
    private WeaponAimManager _weaponAimManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private List<Transform> _nearbyInteractables;

    public void Initialize(Character character)
    {
        _character = character;
    }

    private void Awake()
    {
        weaponParent = transform.Find("WeaponParent");
        _weaponAimManager = weaponParent.GetComponent<WeaponAimManager>();
        _nearbyInteractables = new List<Transform>();
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

    private void UpdateInteractables()
    {
        if (_nearbyInteractables.Count < 2) return;
        _nearbyInteractables = _nearbyInteractables.OrderBy
        (
            i => Vector2.Distance(i.position, transform.position)
        ).ToList();
    }
    public void AddNearbyInteractable(Transform interactable)
    {
        if (_nearbyInteractables.Contains(interactable)) return;
        _nearbyInteractables.Add(interactable);
        Debug.Log(interactable.transform.name + "added to " + name + "interactable pool");
        UpdateInteractables();
    }

    public void RemoveNearbyInteractable(Transform interactable)
    {
        if (!_nearbyInteractables.Contains(interactable)) return;
        _nearbyInteractables.Remove(interactable);
        Debug.Log(interactable.transform.name + "removed from " + name + "interactable pool");
        UpdateInteractables();
    }
    public void Interact()
    {
        if (_nearbyInteractables.Count == 0) return;
        _nearbyInteractables[0].GetComponent<Interactable>().Trigger();
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
