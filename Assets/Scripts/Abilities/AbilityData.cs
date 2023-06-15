using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Abilities", order = 3)]
public class AbilityData : ScriptableObject
{
    public string code;
    public string abilityName;
    public string description;
    [Min(0f)] public float cooldown;
    public List<Effect> externalCastEffects;
    public List<Effect> selfCastEffects;
}
