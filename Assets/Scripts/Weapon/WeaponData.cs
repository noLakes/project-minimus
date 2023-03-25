using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    public string code;
    public string weaponName;
    public string description;
    public GameObject prefab;
    public float attackRate;
}
