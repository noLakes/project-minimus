using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Passive,
    Active
}

[CreateAssetMenu(fileName = "PassiveItem", menuName = "Scriptable Objects/PassiveItem", order = 1)]
public class ItemData : ScriptableObject
{
    public string code;
    public string itemName;
    public ItemType type;
    public GameObject pickupPrefab;
    public Sprite uiSprite;
    public List<Effect> passiveEffects;
    public List<Effect> conferedOnHitEffects;
    public AbilityData onUseAbility;
    [Min(0f)] public float cooldown; // should be 0f if item is passive
}
