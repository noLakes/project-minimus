using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character
{
    protected string uid;
    public CharacterData data { get; protected set; }
    public string code { get; protected set; }
    public Transform transform;
    public int health;
    public int maxHealth;

    List<Weapon> _weapons;
    public List<Weapon> Weapons { get => _weapons; }

    public Character(CharacterData initialData)
    {
        uid = System.Guid.NewGuid().ToString();
        data = initialData;
        code = data.code;
        health = data.health;
        maxHealth = data.health;

        _weapons = new List<Weapon>();

        GameObject g = GameObject.Instantiate(data.prefab) as GameObject;
        transform = g.transform;
        transform.parent = Game.Instance.CHARACTER_CONTAINER;
        g.GetComponent<CharacterManager>().Initialize(this);
    }

    public virtual void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public override string ToString()
    {
        return "{ code: " + code + ", uid: " + uid + " }";
    }

    public void AddWeapon(Weapon newWeapon)
    {
        if(!_weapons.Contains(newWeapon))
        {
            _weapons.Add(newWeapon);
        }
    }

    public void RemoveWeapon(Weapon weapon)
    {
        if(_weapons.Contains(weapon))
        {
            _weapons.Remove(weapon);
        }
    }
}
