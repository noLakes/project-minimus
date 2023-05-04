using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character
{
    private readonly string _uid;
    private CharacterData _data;
    private CharacterStats _baseStats;
    private CharacterStats _activeStats;
    private string _code;
    public readonly Transform Transform;
    private int _health;
    private List<Weapon> _weapons;

    public Character(CharacterData initialData)
    {
        _uid = System.Guid.NewGuid().ToString();
        _data = initialData;
        _baseStats = new CharacterStats(initialData);
        _activeStats = _baseStats;
        _code = _data.code;
        _health = _data.maxHealth;

        _weapons = new List<Weapon>();

        var g = GameObject.Instantiate(_data.prefab) as GameObject;
        Transform = g.transform;
        Transform.parent = Game.Instance.CHARACTER_CONTAINER;
        g.GetComponent<CharacterManager>().Initialize(this);
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

    public CharacterStats Stats => _activeStats;

    public List<Weapon> Weapons => _weapons;

    public override string ToString()
    {
        return "{ Code: " + Code + ", _uid: " + _uid + " }";
    }
}
