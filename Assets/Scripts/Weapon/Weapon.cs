using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    protected string uid;
    public WeaponData data { get; protected set; }
    public string code { get; protected set; }
    public Transform transform;
    public float attackRate { get; protected set; }
    // needs to be implemented/connected...
    public bool isEquipped { get; protected set; }

    public Character owner { get; protected set; }

    public Weapon(WeaponData initialData, Character owner)
    {
        uid = System.Guid.NewGuid().ToString();
        data = initialData;
        code = data.code;
        attackRate = data.attackRate;
        this.owner = owner;
    }

    public void Equip()
    {
        GameObject g = GameObject.Instantiate(data.prefab) as GameObject;
        transform = g.transform;
        transform.parent = owner.transform.Find("WeaponParent");
        g.GetComponent<WeaponManager>().Initialize(this);

        // set equiped?
    }
}
