using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Passive,
    Active
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public string code;
    public string itemName;
    public ItemType type;
    public GameObject pickupPrefab;
    public Sprite uiSprite;
    public List<Effect> passiveEffects;
    public List<Effect> conferredOnHitEffects;
    public AbilityData onUseAbility;
}
