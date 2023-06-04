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
    public int maxHealth;
    public int maxWeaponCount;
    public GameObject prefab;
    public float fovRadius;
    public List<PassiveItemData> startingPassiveItems;
}
