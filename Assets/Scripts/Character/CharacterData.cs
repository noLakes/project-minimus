using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character", order = 1)]
public class CharacterData : ScriptableObject
{
    public string code;
    public string unitName;
    public string description;
    public int health;
    public GameObject prefab;
}
