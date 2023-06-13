using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum WeaponType
{
    AnimationMelee,
    PhysicsMelee,
    Ranged,
    Ability
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    public string code;
    public string weaponName;
    public string description;
    public GameObject prefab;
    public WeaponType type;
    public WeaponStats baseStats;
}
