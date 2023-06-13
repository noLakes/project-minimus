using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Abilities", order = 3)]
public class AbilityData : ScriptableObject
{
    public string code;
    public string abilityName;
    public string description;
    [Range(0f,300f)] public float cooldown;
    public List<Effect> onCastEffects;
}
