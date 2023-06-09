using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character", order = 1)]
public class CharacterData : ScriptableObject
{
    public string code;
    public string characterName;
    public string description;
    public CharacterStats baseStats;
    public GameObject prefab;
    public List<WeaponData> startingWeapons;
    public List<ItemData> startingPassiveItems;
    public AbilityData specialAbility;
    public ItemData startingActiveItem;
}
