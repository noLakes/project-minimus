using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Melee,
    Ranged
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    public string code;
    public string weaponName;
    public string description;
    public GameObject prefab;
    public WeaponType type;
    public int damage;
    public float attackRate;
    public int magazineSize;
    public float reloadTime;

    public List<Effect> onHitEffects;
}
