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
    private ActiveItem _activeItem;
    private ActiveItem _specialAbility; // cannot be changed throughout game
    private Dictionary<string, PassiveItem> _passiveItemInventory;
    
    public Character(CharacterData initialData)
    {
        _uid = System.Guid.NewGuid().ToString();
        _data = initialData;
        _baseStats = _data.baseStats;
        _activeStats = _baseStats;
        _code = _data.code;
        _health = _baseStats.maxHealth;

        if (_data.specialAbility != null)
        {
            _specialAbility = new ActiveItem(_data.specialAbility, this);
        }

        _weapons = new List<Weapon>();
        foreach (var wData in _data.startingWeapons)
        {
            if (_weapons.Count >= Stats.maxWeaponCount) break;
            AddWeapon(new Weapon(wData));
        }

        var g = GameObject.Instantiate(_data.prefab) as GameObject;
        Transform = g.transform;
        Transform.parent = Game.Instance.CHARACTER_CONTAINER;
        g.GetComponent<CharacterManager>().Initialize(this);

        _passiveItemInventory = new Dictionary<string, PassiveItem>();
        foreach (var pItemData in initialData.startingPassiveItems)
        {
            AddPassiveItem(new PassiveItem(pItemData));
        }
    }

    public virtual void SetPosition(Vector3 position)
    {
        Transform.position = position;
    }
    
    public void AddWeapon(Weapon newWeapon)
    {
        if(!_weapons.Contains(newWeapon))
        {
            _weapons.Add(newWeapon);
            Debug.Log(_code + " weapon inventory:");
            foreach (var wep in _weapons)
            {
                Debug.Log(wep.Data.name);
            }
        }
    }

    public void RemoveWeapon(Weapon weapon)
    {
        if(_weapons.Contains(weapon))
        {
            _weapons.Remove(weapon);
        }
    }
    
    public void AddPassiveItem(PassiveItem pItem)
    {
        if (HasPassiveItem(pItem.Data.code))
        {
            Debug.Log(_data.characterName + " already has " + pItem.Data.itemName + " in inventory");
            return;
        }
        
        _passiveItemInventory.Add(pItem.Data.code, pItem);
        Debug.Log("added " + pItem.Data.itemName + " to " + _data.characterName +"'s inventory");
        pItem.ApplyModsToCharacter(this);
    }
    
    public void RemovePassiveItem(string code)
    {
        if (!HasPassiveItem(code)) return;

        var pItem = _passiveItemInventory[code];
        pItem.RemoveModsFromCharacter(this);
        _passiveItemInventory.Remove(code);
        Debug.Log("removed " + pItem.Data.itemName + " from " + _data.characterName +"'s inventory");
    }

    public void RemovePassiveItem(PassiveItem pItem) => RemovePassiveItem(pItem.Data.code);

    public bool HasPassiveItem(string code)
    {
        return _passiveItemInventory.ContainsKey(code);
    }

    public void AddActiveItem(ActiveItem activeItem)
    {
        _activeItem = activeItem;
    }

    public void RemoveActiveItem()
    {
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
    public ActiveItem ActiveItem => _activeItem;
    public ActiveItem SpecialAbility => _specialAbility;

    public override string ToString()
    {
        return "{ Code: " + Code + ", _uid: " + _uid + " }";
    }
}
