using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character
{
    private readonly string _uid;
    protected CharacterData _data;
    private CharacterStats _baseStats;
    private CharacterStats _activeStats;
    private string _code;
    public readonly Transform Transform;
    private int _health;
    private List<Weapon> _weapons;
    private Item _activeItem;
    private AbilityData _specialAbility;
    private Dictionary<string, Item> _passiveItemInventory;
    
    public Character(CharacterData initialData)
    {
        _uid = System.Guid.NewGuid().ToString();
        _data = initialData;
        _baseStats = _data.baseStats;
        _activeStats = _baseStats;
        _code = _data.code;
        _health = _baseStats.maxHealth;
        _specialAbility = _data.specialAbility;

        _weapons = new List<Weapon>();
        
        foreach (var wepData in _data.startingWeapons)
        {
            if (_weapons.Count >= Stats.maxWeaponCount) break;
            AddWeapon(new Weapon(wepData));
        }

        var g = GameObject.Instantiate(_data.prefab);
        Transform = g.transform;
        Transform.parent = Game.Instance.CHARACTER_CONTAINER;
        g.GetComponent<CharacterManager>().Initialize(this);

        if (_data.startingActiveItem != null) _activeItem = new Item(_data.startingActiveItem, this);

        _passiveItemInventory = new Dictionary<string, Item>();
        foreach (var pItemData in initialData.startingPassiveItems)
        {
            AddPassiveItem(pItemData);
        }
    }

    public virtual void SetPosition(Vector3 position)
    {
        Transform.position = position;
    }
    
    public void AddWeapon(Weapon newWeapon)
    {
        if (_weapons.Contains(newWeapon)) return;
        
        _weapons.Add(newWeapon);
        
        string invReadout = _code + " weapon inventory:";
        foreach (var wep in _weapons) invReadout += "\n" + wep.Data.name;
        Debug.Log(invReadout);
    }

    public void RemoveWeapon(Weapon weapon)
    {
        if(_weapons.Contains(weapon)) _weapons.Remove(weapon);
    }
    
    public void AddPassiveItem(ItemData data)
    {
        if (data.type != ItemType.Passive) return;
        
        if (HasPassiveItem(data.code))
        {
            Debug.Log(_data.characterName + " already has " + data.itemName + " in inventory");
            return;
        }
        
        _passiveItemInventory.Add(data.code, new Item(data, this));
        Debug.Log("added " + data.itemName + " to " + _data.characterName +"'s inventory");
    }
    
    public void RemovePassiveItem(string code)
    {
        if (!HasPassiveItem(code)) return;

        var pItem = _passiveItemInventory[code];
        pItem.Unequip();
        _passiveItemInventory.Remove(code);
        Debug.Log("removed " + pItem.Data.itemName + " from " + _data.characterName +"'s inventory");
    }

    public bool HasPassiveItem(string code)
    {
        return _passiveItemInventory.ContainsKey(code);
    }

    public void AssignActiveItem(ItemData data)
    {
        _activeItem = new Item(data, this);
    }

    public void RemoveActiveItem()
    {
        _activeItem.Unequip();
        _activeItem = null;
    }

    public string Code
    {
        get => _code;
    }
    
    public int Health
    {
        get => _health;
        set => _health = Mathf.Min(value, _activeStats.maxHealth);
    }
    
    public int MaxHealth
    {
        get => _activeStats.maxHealth;
        set => _activeStats.maxHealth = Mathf.Max(0, value);
    }

    public float Speed
    {
        get => _activeStats.speed;
        set => _activeStats.speed = value;
    }

    public CharacterStats Stats => _activeStats;

    public List<Weapon> Weapons => _weapons;
    public Item ActiveItem => _activeItem;
    public AbilityData SpecialAbility => _specialAbility;

    public override string ToString()
    {
        return "{ Code: " + Code + ", _uid: " + _uid + " }";
    }
}
