using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    protected Character _character;
    private Weapon _currentWeapon;
    [SerializeField] private Transform weaponParent;
    private CharacterWeaponAimer _characterWeaponAimer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool _isPlayer;

    public virtual void Initialize(Character character)
    {
        _character = character;
    }

    protected virtual void Awake()
    {
        weaponParent = transform.Find("WeaponParent");
        _characterWeaponAimer = weaponParent.GetComponent<CharacterWeaponAimer>();
    }

    void Update()
    {

    }

    public void SetAsPlayer()
    {
        _isPlayer = true;
    }

    public void Damage(int amount)
    {
        _character.Health -= amount;
        if(_isPlayer) EventManager.TriggerEvent("PlayerStatsChange");
        if(_character.Health <= 0) Die();
    }

    public virtual void ReceiveHit(Transform attacker, Vector2 origin)
    {
        // do nothing by default?
        // AI will respond with override method
    }

    public void Heal(int amount)
    {
        _character.Health += amount;
        if(_isPlayer) EventManager.TriggerEvent("PlayerStatsChange");
    }

    private void Die()
    {
        //Debug.Log(transform.name + " is dead.");
        // update Game Instance CHARACTERS list if needed
        Destroy(transform.gameObject);
    }

    public void Attack() => _currentWeapon.Attack();
    
    public void Attack(Vector2 location)
    {
        _currentWeapon.Attack(location);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if(_currentWeapon != null && weapon != _currentWeapon) _currentWeapon.Unequip();

        _currentWeapon = weapon;
        _currentWeapon.Equip(_character);
        
        _characterWeaponAimer.UpdateSpriteRenderers(
            _currentWeapon.Manager.SpriteRenderer,
            spriteRenderer
            );

        _characterWeaponAimer.SetWeapon(weapon);
        if(_isPlayer) EventManager.TriggerEvent("PlayerWeaponChange");
    }

    public Weapon AddWeapon(Weapon newWeapon)
    {
        _character.AddWeapon(newWeapon);

        return newWeapon;
    }

    public Weapon DropWeapon()
    {
        var droppedWeapon = _currentWeapon;
        _currentWeapon = null;
        droppedWeapon.Unequip();
        Character.RemoveWeapon(droppedWeapon);
        _characterWeaponAimer.SetWeapon(null);

        Vector2 mouseDir = (Utility.GetMouseWorldPosition2D() - (Vector2)transform.position).normalized;
        Weapon.SpawnInWorld(droppedWeapon, (Vector2)transform.position + mouseDir);
        return droppedWeapon;
    }

    public void SwitchWeapon()
    {
        if (_character.Weapons.Count < 2)
        {
            // respond to cant switch
            return;
        }
        
        //_currentWeapon.Interrupt? if interrupt is being used;
        var nextWeaponIndex = _character.Weapons.IndexOf(_currentWeapon) + 1;
        if (nextWeaponIndex == _character.Weapons.Count) nextWeaponIndex = 0;
        
        EquipWeapon(_character.Weapons[nextWeaponIndex]);
    }

    public float Size
    {
        get => spriteRenderer.GetComponent<Renderer>().bounds.size.x;
    }
    
    public Character Character => _character;
    public Weapon CurrentWeapon => _currentWeapon;
    public bool IsPlayer => _isPlayer;
}
