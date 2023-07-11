using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    protected Character _character;
    private Weapon _currentWeapon;
    private AbilityManager _specialAbilityManager;
    private Item _currentActiveItem;
    [SerializeField] private Transform weaponParent;
    private CharacterWeaponAimer _characterWeaponAimer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool _isPlayer;
    [SerializeField] private List<Effect> _activeEffects;

    public virtual void Initialize(Character character)
    {
        _character = character;
        
        if (_character.Weapons.Count > 0)
        {
            EquipWeapon(_character.Weapons[0]);
        }

        if (character.SpecialAbility != null)
        {
            _specialAbilityManager = transform.AddComponent<AbilityManager>();
            _specialAbilityManager.Initialize(_character.SpecialAbility, _character);
        }

        if (_character.ActiveItem != null)
        {
            _currentActiveItem = _character.ActiveItem;
        }
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
        // do nothing by default
        // AI will respond with override method
    }

    public void Heal(int amount)
    {
        if (amount < 0f) return; // damage not allowed via heal method
        _character.Health += amount;
        if(_isPlayer) EventManager.TriggerEvent("PlayerStatsChange");
    }

    private void Die()
    {
        Destroy(transform.gameObject);
    }

    public void Attack() => _currentWeapon.Attack();
    
    public void Attack(Vector2 location)
    {
        _currentWeapon.Attack(location);
    }

    public void UseAbility(Vector2 location)
    {
        if (_specialAbilityManager == null) return; 
        
        _specialAbilityManager.Trigger(location);
    }

    public void UseActiveItem(Vector2 location)
    {
        _currentActiveItem?.Use(location);
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
        if (_character.Weapons.Count < 2) return;
        
        var nextWeaponIndex = _character.Weapons.IndexOf(_currentWeapon) + 1;
        if (nextWeaponIndex == _character.Weapons.Count) nextWeaponIndex = 0;
        
        EquipWeapon(_character.Weapons[nextWeaponIndex]);
    }

    public void AddItem(ItemData data)
    {
        if (data.type == ItemType.Active)
        {
            if (_character.ActiveItem != null)
            {
                var oldItemData = _character.ActiveItem.Data;
                _character.RemoveActiveItem();
                ItemPickup.Create(oldItemData, transform.position);
            }
            
            _character.AssignActiveItem(data);
            _currentActiveItem = _character.ActiveItem;
        }
        else _character.AddPassiveItem(data);
    }
    
    public void AddEffect(Effect e)
    {
        _activeEffects.Add(e);
        var printout = transform.name + "active effects: ";
        foreach (var effect in _activeEffects)
        {
            printout += effect.name + " / ";
        }
        Debug.Log(printout);
    }

    public bool RemoveEffect(Effect e)
    {
        var result = _activeEffects.Remove(e);
        var printout = "active effects after removal: ";
        foreach (var effect in _activeEffects)
        {
            printout += effect.name + " / ";
        }
        Debug.Log(printout);
        return result;
    }

    public bool AffectedBy(Effect e)
    {
        return _activeEffects.Any(effect => effect.name == e.name);
    }

    public virtual void OnSpeedChange()
    {
        // do nothing by default
    }

    public float GetSize() // returns full width or diameter of character
    {
        if (TryGetComponent<CircleCollider2D>(out var circleCollider2D))
        {
            return circleCollider2D.radius * 2f;
        }
        
        return spriteRenderer.GetComponent<Renderer>().bounds.size.x * 2f;
    } // does not currently handle any complicated characters with multiple meshes or hit boxes

    public Character Character => _character;
    public Weapon CurrentWeapon => _currentWeapon;
    public AbilityManager SpecialAbilityManager => _specialAbilityManager;
    public Item CurrentActiveItem => _currentActiveItem;
    public bool IsPlayer => _isPlayer;
}
